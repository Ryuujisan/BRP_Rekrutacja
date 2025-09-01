using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UiInput
{
    public static class UISelectHelper
    {
        public static IEnumerator GiveFocus(Selectable sel, int frames = 6)
        {
            for (var i = 0; i < frames; i++) yield return null;

            var es = EventSystem.current;
            if (!es || !sel) yield break;

            es.SetSelectedGameObject(null);
            if (sel.IsActive() && sel.interactable)
            {
                sel.Select();
                es.SetSelectedGameObject(sel.gameObject);
            }
        }
    }
}