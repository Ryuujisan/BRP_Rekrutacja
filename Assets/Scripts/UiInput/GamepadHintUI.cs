using ScriptableObjectsScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UiInput
{
    public enum EHintAction
    {
        Confirm,   // X (PS) / A (Xbox)
        Cancel,    // O (PS) / B (Xbox)
        Options,   // Options / Menu
        Back,      // Create / View
        Touchpad   // Touchpad (PS only)
    }
    public class GamepadHintUI : MonoBehaviour
    {
        [SerializeField]
        private Image _hintImage;

        [SerializeField]
        private EHintAction _action;
        void Start()
        {
            InputDeviceWatcher.I.OnGamepadModeChanged += Toggle;

            Toggle(InputDeviceWatcher.UsingGamepad);
            _hintImage.sprite = GamepadIcons.I.GetIcon(_action);
        }

        void OnDestroy()
        {
            InputDeviceWatcher.I.OnGamepadModeChanged -= Toggle;
        }

        public void Toggle(bool visible)
        {
            Debug.Log($"[HintUI] event received → visible={visible}");
            gameObject.SetActive(visible);
        }
    }
}