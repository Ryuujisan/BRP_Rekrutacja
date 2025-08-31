using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager I { get; private set; }
        
        // === EVENTY for UI ===
        public event Action OnConfirm;
        public event Action OnCancel;
        public event Action OnPause;
        public event Action OnInventory;

        private InputMaster _inputMaster;

        
        private void Awake()
        {
            if (I == null)
                I = this;

            _inputMaster = new();
            
            _inputMaster.UI.Pause.performed += ctx =>
            {
                Debug.Log("Pasue Press");
                OnPause?.Invoke();
            };

            _inputMaster.UI.Inventory.performed += ctx =>
            {
                Debug.Log("Inventory Press");
                OnInventory?.Invoke();
            };

            _inputMaster.UI.Cancel.performed += ctx =>
            {
                Debug.Log("Cancel Press");
                OnCancel?.Invoke();
            };
        }
        void OnEnable()
        {
            _inputMaster.Enable();            
        }
        private void OnDestroy()
        {
            _inputMaster.Dispose();
        }
    
    }
}