using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoulsItemAutoDeselect : MonoBehaviour
{
    [SerializeField]
    private SoulsGridNavigator _gridNavigator;
    Selectable selectable;

    void Awake() => selectable = GetComponent<Selectable>();

    void OnDisable() => MoveFocusIfSelected();
    void OnTransformParentChanged() => MoveFocusIfSelected();

    void MoveFocusIfSelected()
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