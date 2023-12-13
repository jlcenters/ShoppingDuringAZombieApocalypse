using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
/*
 * 
 * 
    PLAYER INPUT CONTROLLER FUNCTIONS:
        utilizes new input system
        stores events
        checks movement axis
        checks for pause
        checks for crouch toggle
 *
 *
 */
public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get; private set; }
    private UserInputActions userInputActions;

    //player input events
    public event Action OnPause;
    public event Action OnToggleCrouch;
    public event Action OnToggleSprint;
    public event Action OnJump;
    public event Action OnToggleShoot;
    public event Action OnWeaponStrike;
    public event Action OnMeleeAttack;

    //user input actions toggle var
    //private bool uiaEnabled = true;



    private void Awake()
    {
        Instance = this;

        //on awake set and enable player input reader
        userInputActions = new UserInputActions();
        userInputActions.PlayerInput.Enable();

        //subscribe input actions to events in game
        userInputActions.PlayerInput.Pause.performed += Pause_performed;
        userInputActions.PlayerInput.Crouch.performed += Crouch_toggled;
        userInputActions.PlayerInput.Sprint.performed += Sprint_toggled;
        userInputActions.PlayerInput.Sprint.canceled += Sprint_toggled;
        userInputActions.PlayerInput.Jump.performed += Jump_performed;
        userInputActions.PlayerInput.ShootWeapon.performed += ShootWeapon_toggled;
        userInputActions.PlayerInput.ShootWeapon.canceled += ShootWeapon_toggled;
        userInputActions.PlayerInput.WeaponStrike.performed += WeaponStrike_performed;
        userInputActions.PlayerInput.MeleeAttack.performed += MeleeAttack_performed;
    }
    private void OnDestroy()
    {
        //remove instance and reader
        //Instance = null;
        userInputActions.Dispose();

        //unsubscribe input events
        userInputActions.PlayerInput.Pause.performed -= Pause_performed;
        userInputActions.PlayerInput.Crouch.performed -= Crouch_toggled;
        userInputActions.PlayerInput.Sprint.performed -= Sprint_toggled;
        userInputActions.PlayerInput.Sprint.canceled -= Sprint_toggled;
        userInputActions.PlayerInput.ShootWeapon.performed -= ShootWeapon_toggled;
        userInputActions.PlayerInput.ShootWeapon.canceled -= ShootWeapon_toggled;
        userInputActions.PlayerInput.WeaponStrike.performed -= WeaponStrike_performed;
        userInputActions.PlayerInput.MeleeAttack.performed -= MeleeAttack_performed;
    }



    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        //TODO: pause all user input actions except pause 
        OnPause?.Invoke();
    }
    public Vector2 GetMovementNormalized()
    {
        Vector2 inputDirection = userInputActions.PlayerInput.Move.ReadValue<Vector2>();

        return inputDirection.normalized;
    }
    private void Crouch_toggled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnToggleCrouch?.Invoke();
    }
    private void Sprint_toggled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnToggleSprint?.Invoke();
    }
    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }
    private void ShootWeapon_toggled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnToggleShoot?.Invoke();
    }
    private void WeaponStrike_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnWeaponStrike?.Invoke();
    }
    private void MeleeAttack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMeleeAttack?.Invoke();
    }
    /*private void ToggleUserInputs()
    {
        uiaEnabled = !uiaEnabled;
        if(uiaEnabled)
        {
            userInputActions.Enable();
        }
        else
        {
            userInputActions.Disable();
        }
    }*/
}
