using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UiInput
{
    public class UIFocusSticky : MonoBehaviour
    {
        public Selectable defaultSelectable;
        private GameObject last;

        private void LateUpdate()
        {
            var es = EventSystem.current;
            if (!es) return;

            var cur = es.currentSelectedGameObject;

            if (cur == null || !cur.activeInHierarchy)
            {
                if (last != null)
                    es.SetSelectedGameObject(last);
                else if (defaultSelectable)
                    es.SetSelectedGameObject(defaultSelectable.gameObject);
            }
        }

        private void OnEnable()
        {
            last = defaultSelectable ? defaultSelectable.gameObject : null;
            StartCoroutine(UISelectHelper.GiveFocus(defaultSelectable, 2));
        }
    }
}