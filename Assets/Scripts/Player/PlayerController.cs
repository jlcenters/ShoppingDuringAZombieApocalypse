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

    private Rigidbody rb;

    [Header("Movement Speeds")]
    [SerializeField] private float joggingSpeed;
    [SerializeField] private float sneakingSpeed;
    [SerializeField] private float sprintingSpeed;
    [SerializeField] private float currentMovementSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float rotateSpeed;

    [Header("Horizontal Movement")]
    [SerializeField] private float playerStandHeight = 2;
    [SerializeField] private float playerCrouchHeight;
    [SerializeField] private float currentPlayerHeight;

    [Header("Vertical Movement")]
    [SerializeField] private float drag;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float fallMultiplier;

    [Header("Combat")]
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int playerHealth;
    [SerializeField] private int playerStamina;
    [SerializeField] private float attackSpeed;

    [Header("Interactions")]
    [SerializeField] private LayerMask groceriesLayer;
    [SerializeField] private LayerMask healthPackLayer;
    [SerializeField] private float interactDistance;

    //bools
    private bool canMove = true;
    private bool canToggleCrouch = false;
    private bool jumpExecuted = false;
    private bool shootExecuted = false;
    private bool isShooting = false;

    private void Awake()
    {
        Instance = this;

        currentPlayerHeight = playerStandHeight;
        currentMovementSpeed = joggingSpeed;
        transform.localScale = new(transform.localScale.x, currentPlayerHeight, transform.localScale.z);
        
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        PlayerInputController.Instance.OnToggleCrouch += PlayerInputController_OnToggleCrouch;
        PlayerInputController.Instance.OnToggleSprint += PlayerInputController_OnToggleSprint;
        PlayerInputController.Instance.OnJump += PlayerInputController_OnJump;
        PlayerInputController.Instance.OnToggleShoot += PlayerInputController_OnToggleShoot;
        PlayerInputController.Instance.OnWeaponStrike += PlayerInputController_OnWeaponStrike;
        PlayerInputController.Instance.OnMeleeAttack += PlayerInputController_OnMeleeAttack;
    }
    private void Update()
    {
        if (GameController.Instance.IsCurrentState(GameStates.PlayGame))
        {
            //attempt to crouch
            if (canToggleCrouch)
            {
                //takes new height and lerps
                StartCoroutine(Crouch(new(transform.localScale.x, currentPlayerHeight, transform.localScale.z)));
            }

            //attempt to jump
            if(jumpExecuted && IsGrounded())
            {
                StartCoroutine(Jump());
            }
            //attempt to move
            else if (canMove)
            {
                Vector2 inputDirection = PlayerInputController.Instance.GetMovementNormalized();
                Move(inputDirection.x, inputDirection.y);
            }


        }
    }
    private void FixedUpdate()
    {
        if(GameController.Instance.IsCurrentState(GameStates.PlayGame))
        {
            //attempt to shoot
            if (isShooting && shootExecuted)
            {
                StartCoroutine(ShootWeapon());
            }
        }
    }
    private void OnDestroy()
    {
        Instance = null;
        //PlayerInputController.Instance.OnToggleCrouch -= PlayerInputController_OnToggleCrouch;
    }



    private void Move(float x, float z)
    {
        Vector3 movementDirection = new Vector3(x, 0, z).normalized;
         
        transform.position += movementDirection * (currentMovementSpeed * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotateSpeed);
    }
    private bool CheckToMove(Vector3 direction)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * transform.localScale.y, 1f, direction);
    }
    private void PlayerInputController_OnToggleCrouch()
    {
        if (currentPlayerHeight == playerStandHeight)
        {
            currentPlayerHeight /= 2f;
            currentMovementSpeed = crouchSpeed;
        }
        else
        {
            currentPlayerHeight = playerStandHeight;
            currentMovementSpeed = joggingSpeed;
        }
        canToggleCrouch = !canToggleCrouch;
    }
    private IEnumerator Crouch(Vector3 newSize)
    {
        transform.localScale = Vector3.Lerp(transform.localScale, newSize, Time.deltaTime * crouchSpeed);
        yield return new WaitForSeconds(crouchSpeed);
        canToggleCrouch = !canToggleCrouch;
    }
    private void PlayerInputController_OnToggleSprint()
    {
        if(currentMovementSpeed == sneakingSpeed)
        {
            //cannot sprint from a sneak
            return;
        }
        else if(currentMovementSpeed == joggingSpeed)
        {
            //begin sprinting
            currentMovementSpeed = sprintingSpeed;
        }
        else if(currentMovementSpeed == sprintingSpeed)
        {
            //stop sprinting
            currentMovementSpeed = joggingSpeed;
        }
    }
    private void PlayerInputController_OnJump()
    {
        //cannot jump if sneaking
        if(IsGrounded())
        {
            jumpExecuted = !jumpExecuted;
            canMove = !canMove;
        }
    }
    private IEnumerator Jump()
    {
        //toggle off jump check in update
        jumpExecuted = !jumpExecuted;

        //jump and wait
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Force);
        yield return new WaitForSeconds(0.5f);

        //when player hits ground, toggle can jump and can move bools
        yield return new WaitWhile(() => !IsGrounded());
        canMove = !canMove;
    }
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, 1f);
    }
    private void PlayerInputController_OnToggleShoot()
    {
        shootExecuted = !shootExecuted;
        isShooting = !isShooting;
    }
    private IEnumerator ShootWeapon()
    {
        shootExecuted = !shootExecuted;
        Debug.Log("brrap");
        yield return new WaitForSeconds(0.5f);
        shootExecuted = !shootExecuted;
    }
    private void PlayerInputController_OnWeaponStrike()
    {
        Debug.Log("whap");
    }
    private void PlayerInputController_OnMeleeAttack()
    {
        Debug.Log("swish");
    }
}
