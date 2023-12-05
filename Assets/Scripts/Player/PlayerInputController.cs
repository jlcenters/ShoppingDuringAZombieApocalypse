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
        checks movement axis
        checks for pause
 *
 *
 */
public class PlayerInputController : MonoBehaviour
{
    public static PlayerInputController Instance { get; private set; }
    private UserInputActions userInputActions;

    //player input events
    public event Action OnPause;



    private void Awake()
    {
        Instance = this;

        //on awake set and enable player input reader
        userInputActions = new UserInputActions();
        userInputActions.PlayerInput.Enable();

        //subscribe input actions to events in game
        userInputActions.PlayerInput.Pause.performed += Pause_performed;
    }
    private void OnDestroy()
    {
        //remove instance and reader
        Instance = null;
        userInputActions.Dispose();

        //unsubscribe input events
        userInputActions.PlayerInput.Pause.performed -= Pause_performed;
    }



    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke();
    }
    public Vector2 GetMovementNormalized()
    {
        Vector2 inputDirection = userInputActions.PlayerInput.Move.ReadValue<Vector2>();

        return inputDirection.normalized;
    }
}
