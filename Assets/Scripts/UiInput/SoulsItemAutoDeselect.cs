using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulsItemAutoDeselect : MonoBehaviour
{
    [SerializeField]
    private SoulsGridNavigator _gridNavigator;

    private Selectable selectable;

    private void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    private void OnDisable()
    {
        MoveFocusIfSelected();
    }

    private void OnTransformParentChanged()
    {
        MoveFocusIfSelected();
    }

    private void MoveFocusIfSelected()
    {
        var es = EventSystem.current;
        if (!es || !selectable) return;

        if (es.currentSelectedGameObject == gameObject)
        {
            var n = selectable.navigation;
            var next = n.selectOnRight ?? n.selectOnDown ?? n.selectOnLeft ?? n.selectOnUp;
            if (next && next.IsActive() && next.interactable)
            {
                es.SetSelectedGameObject(null);
                next.Select();
                es.SetSelectedGameObject(next.gameObject);
            }
            else
            {
                _gridNavigator.RebuildNavigation();
            }
        }
    }
}