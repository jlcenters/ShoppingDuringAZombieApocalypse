using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 
 * 
    PLAYER CONTROLLER FUNCTIONS:
        moves player
        attacks
        checks interactions
        checks hp when damaged
 *
 *
 */
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [Header("Horizontal Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float playerHeight = 2; //crouching height = half of player height
    private bool canMove = false;

    [Header("Vertical Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float fallMultiplier;

    [Header("Combat")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerStamina;
    [SerializeField] private float attackSpeed;

    [Header("Interactions")]
    //private Rigidbody rb;
    [SerializeField] private LayerMask groceriesLayer;
    [SerializeField] private LayerMask healthPackLayer;
    [SerializeField] private float interactDistance;



    private void Awake()
    {
        Instance = this;

        //rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(GameController.Instance.IsCurrentState(GameStates.PlayGame))
        {
            //attempt to move
            Vector2 inputDirection = PlayerInputController.Instance.GetMovementNormalized();
            Move(inputDirection.x, inputDirection.y);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }



    public void ToggleMovement()
    {
        canMove = !canMove;
    }
    private void Move(float x, float z)
    {
        Vector3 movementDirection = new Vector3(x, 0, z).normalized;

        transform.position += movementDirection * (movementSpeed * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotateSpeed);
    }
    private bool CheckToMove(Vector3 direction)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * transform.localScale.y, 1f, direction);
    }
}
