using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = System.Numerics.Vector2;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;

    private InputAction touchPositionAction;
    private InputAction touchPressAction;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        touchPressAction = _playerInput.actions["TouchPress"];
        touchPositionAction = _playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        touchPressAction.performed += TouchPressed;
    }

    private void OnDisable()
    {
        touchPressAction.performed -= TouchPressed;
    }

    private void TouchPressed(InputAction.CallbackContext context)
    {
        /*Vector3 position = Camera.main.ScreenToWorldPoint(touchPositionAction.ReadValue<Vector2>());
        position.z*/
    }
}
