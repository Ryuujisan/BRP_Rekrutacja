using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UiInput
{
    public static class UISelectHelper
    {
        public static IEnumerator SelectNextFrame(Selectable sel)
        {
            yield return null; 
            if (!EventSystem.current) yield break;

            EventSystem.current.SetSelectedGameObject(null);
            if (sel && sel.IsActive() && sel.interactable)
                EventSystem.current.SetSelectedGameObject(sel.gameObject);
        }
    }
}