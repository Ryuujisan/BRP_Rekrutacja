using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool deselectOnExit = false; // zwykle false

    Selectable sel;

    void Awake() => sel = GetComponent<Selectable>();

    public void OnPointerEnter(PointerEventData e)
    {
        if (!EventSystem.current || sel == null) return;
        if (sel.IsActive() && sel.interactable)
            EventSystem.current.SetSelectedGameObject(sel.gameObject); // hover == selected
    }

    public void OnPointerExit(PointerEventData e)
    {
        if (!deselectOnExit) return;
        if (EventSystem.current && EventSystem.current.currentSelectedGameObject == gameObject)
            EventSystem.current.SetSelectedGameObject(null);
    }
}