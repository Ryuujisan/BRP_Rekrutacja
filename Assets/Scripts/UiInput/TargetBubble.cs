using UnityEngine;
using UnityEngine.UI;

namespace UiInput
{
    //[RequireComponent(typeof(Button))]
    public class TargetBubble : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        private void OnEnable()
        {
            if (TargetSelectedManager.I) TargetSelectedManager.I.Register(_button);
        }

        private void OnDestroy()
        {
            if (TargetSelectedManager.I) TargetSelectedManager.I.UnRegister(_button);
        }

        public void ConfirmSelection()
        {
            TargetSelectedManager.I.LockSelection(_button, transform.parent);
        }
    }
}