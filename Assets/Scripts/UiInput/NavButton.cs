using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UiInput
{
    public class NavButton : Button
    {
        public override void OnSelect(BaseEventData e)
        {
            base.OnSelect(e);
            DoStateTransition(SelectionState.Highlighted, false);
        }

        public override void OnDeselect(BaseEventData e)
        {
            base.OnDeselect(e);
            DoStateTransition(SelectionState.Normal, false);
        }
    }
}