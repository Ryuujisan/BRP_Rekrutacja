using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UiInput
{
    public class TargetSelectedManager : MonoBehaviour
    {
        #region Singleton
        public static TargetSelectedManager I { get; private set; }
        #endregion Singleton
        Selectable _lastWorldSelected; 
        private readonly List<Button> _targets = new();
        
        private void Awake()
        {
            if (I == null)
            {
                I = this;
            } else if(I != this) Destroy(gameObject);
        }

        public void Register(Button button)
        {
            if (!_targets.Contains(button))
            {
                _targets.Add(button);
            }

            RebuildNav();
            
            SelectedNearestButton();
        }

        public void SelectedNearestButton()
        {
            if(EventSystem.current.currentSelectedGameObject == null && _targets.Count > 0)
            {
                var first = _targets.OrderBy(o => o.transform.position.x).First();
                StartCoroutine(UISelectHelper.GiveFocus(first));
            }
        }

        public void UnRegister(Button button)
        {
            bool wasSelected = EventSystem.current.currentSelectedGameObject == button.gameObject;
            _targets.Remove(button);
            RebuildNav();

            if (wasSelected)
            {
                SelectedNearestButton();
            }
        }

        private void RebuildNav()
        {
            var list = _targets.Where(t => t && t.gameObject.activeInHierarchy && t.interactable)
                .OrderBy(t => t.transform.position.x).ToList();

            for (int i = 0; i < list.Count; i++)
            {
                var nav = new Navigation { mode = Navigation.Mode.Explicit };
                if (i > 0) nav.selectOnLeft = list[i - 1];
                if (i < list.Count - 1) nav.selectOnRight = list[i + 1];
                
                nav.selectOnUp = nav.selectOnDown = null;
                list[i].navigation = nav;
            }
        }
        public void LockSelection(Button chosen, Transform scopeParent = null)
        {
            foreach (var b in _targets.ToList())
            {
                if (!b) { _targets.Remove(b); continue; }
                if (b == chosen) continue;
                if (scopeParent && !b.transform.IsChildOf(scopeParent)) continue;

                b.interactable = false;
                b.navigation = new Navigation { mode = Navigation.Mode.None };
            }

            chosen.navigation = new Navigation { mode = Navigation.Mode.None };
            EventSystem.current.SetSelectedGameObject(chosen.gameObject);
        }
        
        public void SetWorldActive(bool active)
        {

            if (!active && EventSystem.current)
                _lastWorldSelected = EventSystem.current.currentSelectedGameObject
                    ? EventSystem.current.currentSelectedGameObject.GetComponent<Selectable>()
                    : null;

            foreach (var b in _targets)
            {
                if (!b) continue;
                b.interactable = active;

                var nav = b.navigation;
                nav.mode = active ? Navigation.Mode.Explicit : Navigation.Mode.None;
                b.navigation = nav;
            }

            if (active)
            {

                RebuildNav();
                StartCoroutine(FocusWorldNextFrame());
            }
            else
            {

                if (EventSystem.current &&
                    _targets.Any(t => t && EventSystem.current.currentSelectedGameObject == t.gameObject))
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }
        
        IEnumerator FocusWorldNextFrame()
        {
            yield return null;
            yield return null;

            var es = EventSystem.current;
            if (!es) yield break;


            var candidate = _lastWorldSelected && _lastWorldSelected.IsActive() && _lastWorldSelected.interactable
                ? _lastWorldSelected
                : GetNearestOrFirst();  

            if (candidate)
            {
                es.SetSelectedGameObject(null);
                candidate.Select();
                es.SetSelectedGameObject(candidate.gameObject);
            }
        }

        Selectable GetNearestOrFirst()
        {
            return _targets.FirstOrDefault(t => t && t.IsActive() && t.interactable);
        }
    }
}