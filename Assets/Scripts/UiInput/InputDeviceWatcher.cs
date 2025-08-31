using System;
using UiInput;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum InputMode { None, Gamepad, Mouse }

public class InputDeviceWatcher : MonoBehaviour
{
    public static InputDeviceWatcher I { get;  private set; }

    public event Action<bool> OnGamepadModeChanged;

    // For test
    [SerializeField] 
    UnityEvent<bool> onGamepadModeChangedInspector;

    public static InputMode CurrentMode { get; private set; } = InputMode.None;
    public static bool UsingGamepad => CurrentMode == InputMode.Gamepad;

    [Header("Config")]
    public float gamepadHintTimeout = 3f;
    public float mouseMoveEpsilon = 0.05f;

    float _timer;
    
    void SetMode(InputMode next, InputDeviceWatcher who = null)
    {
        if (next == CurrentMode) return;

        var prev = CurrentMode;
        CurrentMode = next;

        Debug.Log($"[IDW] prev = {prev}, current = {CurrentMode}  // fire event");
        OnGamepadModeChanged?.Invoke(UsingGamepad);
        if (who) who.onGamepadModeChangedInspector?.Invoke(UsingGamepad);
        if (CurrentMode == InputMode.Gamepad)
        {
            TargetSelectedManager.I.SelectedNearestButton();
        }
    }

    void Awake()
    {
        if (I == null)
        {
            I = this;
        }
    }

    void OnEnable()
    {
        SetMode(InputMode.Mouse, this);
    }

    void Update()
    {
        var gp = Gamepad.current;
        if (gp != null && WasAnyGamepadAction(gp))
        {
            SetMode(InputMode.Gamepad, this);
            if (gamepadHintTimeout > 0f) _timer = gamepadHintTimeout;
            return;
        }

        var ms = Mouse.current;
        if (ms != null && (
            ms.leftButton.wasPressedThisFrame ||
            ms.rightButton.wasPressedThisFrame ||
            ms.middleButton.wasPressedThisFrame ||
            ms.scroll.ReadValue().sqrMagnitude > 0 ||
            ms.delta.ReadValue().sqrMagnitude > mouseMoveEpsilon))
        {
            SetMode(InputMode.Mouse, this);
            _timer = 0f;
            return;
        }

        // auto-hide
        if (gamepadHintTimeout > 0f && CurrentMode == InputMode.Gamepad)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                SetMode(InputMode.Mouse, this);
        }
    }

    static bool WasAnyGamepadAction(Gamepad gp)
    {
        if (gp.buttonSouth.wasPressedThisFrame || gp.buttonNorth.wasPressedThisFrame ||
            gp.buttonWest.wasPressedThisFrame  || gp.buttonEast.wasPressedThisFrame  ||
            gp.startButton.wasPressedThisFrame || gp.selectButton.wasPressedThisFrame ||
            gp.leftShoulder.wasPressedThisFrame || gp.rightShoulder.wasPressedThisFrame ||
            gp.leftStickButton.wasPressedThisFrame || gp.rightStickButton.wasPressedThisFrame ||
            gp.dpad.up.wasPressedThisFrame || gp.dpad.down.wasPressedThisFrame ||
            gp.dpad.left.wasPressedThisFrame || gp.dpad.right.wasPressedThisFrame)
            return true;

        if (gp.leftStick.ReadValue().sqrMagnitude  > 0.01f) return true;
        if (gp.rightStick.ReadValue().sqrMagnitude > 0.01f) return true;
        if (gp.dpad.ReadValue() != Vector2.zero)            return true;
        if (gp.leftTrigger.ReadValue()  > 0.2f)             return true;
        if (gp.rightTrigger.ReadValue() > 0.2f)             return true;

        return false;
    }
}
