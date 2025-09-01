using System;
using UnityEngine;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        private InputMaster _inputMaster;
        public static InputManager I { get; private set; }


        private void Awake()
        {
            if (I == null)
                I = this;

            _inputMaster = new InputMaster();

            _inputMaster.UI.Pause.performed += ctx =>
            {
                OnPause?.Invoke();
            };

            _inputMaster.UI.Inventory.performed += ctx =>
            {
                OnInventory?.Invoke();
            };

            _inputMaster.UI.Cancel.performed += ctx =>
            {
                OnCancel?.Invoke();
            };
        }

        private void OnEnable()
        {
            _inputMaster.Enable();
        }

        private void OnDestroy()
        {
            _inputMaster.Dispose();
        }

        // === EVENTY for UI ===
        public event Action OnConfirm;
        public event Action OnCancel;
        public event Action OnPause;
        public event Action OnInventory;
    }
}