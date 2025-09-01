using UiInput;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

namespace ScriptableObjectsScripts
{
    [CreateAssetMenu(fileName = "GamepadIcons", menuName = "UI/Input/Gamepad Icon Set")]
    public class GamepadIcons : ScriptableObject
    {
        public static GamepadIcons i;


        [Header("PlayStation Icons")]
        public Sprite psCross; // ✕ (Confirm)

        public Sprite psCircle; // ◯ (Cancel)
        public Sprite psOptions; // Options
        public Sprite psCreate; // Create/Share
        public Sprite psTouchpad; // Touchpad

        [Header("Xbox Icons")]
        public Sprite xbA;

        public Sprite xbB;
        public Sprite xbMenu;
        public Sprite xbView;

        public static GamepadIcons I
        {
            get
            {
                if (i == null) i = Resources.Load<GamepadIcons>("GamepadIcons");

                return i;
            }
        }

        /// <summary>
        ///     Returns the appropriate icon for the given action, based on the current controller.
        /// </summary>
        public Sprite GetIcon(EHintAction action)
        {
            var isPS = IsPlayStation(Gamepad.current);

            switch (action)
            {
                case EHintAction.Confirm: return isPS ? psCross : xbA;
                case EHintAction.Cancel: return isPS ? psCircle : xbB;
                case EHintAction.Options: return isPS ? psOptions : xbMenu;
                case EHintAction.Back: return isPS ? psCreate : xbView;
                case EHintAction.Touchpad: return isPS ? psTouchpad : null; // Xbox dont supported
                default: return null;
            }
        }

        private static bool IsPlayStation(Gamepad gp)
        {
            if (gp == null) return false;

            if (gp is DualSenseGamepadHID) return true;
            if (gp is DualShockGamepad) return true;

            var name = (gp.displayName ?? gp.name ?? "").ToLower();
            return name.Contains("dual") || name.Contains("playstation") || name.Contains("ps5") ||
                   name.Contains("ps4");
        }
    }
}