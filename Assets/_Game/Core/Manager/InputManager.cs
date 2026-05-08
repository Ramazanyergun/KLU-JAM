using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private PlayerInputAction inputActions;
    [Header("Movement Inputs")]
    public Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;

    public bool isCrouchedWalking;
    public bool isSprinting;

    [Header("Jump Input")]
    public bool isJumped;

    [Header("Combat Input")]
    public bool isAttacking;
    public bool isDefensing;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            Instance = this;

        inputActions = new PlayerInputAction();
    }


    void OnEnable()
    {
        inputActions.Locomotion.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        inputActions.Locomotion.Movement.canceled += _ => movementInput = Vector2.zero;



        inputActions.Locomotion.Sprint.started += i => isSprinting = true;
        inputActions.Locomotion.Sprint.canceled += i => isSprinting = false;

        inputActions.Locomotion.Jump.started += i => isJumped = true;
        inputActions.Locomotion.Jump.canceled += i => isJumped = false;


        inputActions.Combat.Attack.started += i => isAttacking = true;
        inputActions.Combat.Attack.canceled += i => isAttacking = false;


        inputActions.Combat.Defense.started += i => isDefensing = true;
        inputActions.Combat.Defense.canceled += i => isDefensing = false;

        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
    }


}
