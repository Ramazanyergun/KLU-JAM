using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private float m_sprintSpeed = 8f;
    [SerializeField] private float m_acceleration = 0.1f;
    [SerializeField] private float m_maxEnergyValue = 100;
    [SerializeField] private float m_currentEnergyValue;

    [SerializeField] private float m_energyRegenRate = 10f;
    [SerializeField] private float m_sprintEnergyCost;
    [Header("Jump Settings")]
    [SerializeField] private float m_jumpForce = 10f;

    [Header("Detection")]
    [SerializeField] private Transform m_groundCheckTransform;
    [SerializeField] private float m_groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask m_groundLayer;

    private Rigidbody2D m_playerRB;
    private Vector2 m_velocityReference = Vector2.zero;
    private bool m_isGrounded;

    void Awake()
    {
        m_playerRB = GetComponent<Rigidbody2D>();
        m_currentEnergyValue = m_maxEnergyValue;
    }

    void FixedUpdate()
    {
        m_isGrounded = Physics2D.OverlapCircle(
           m_groundCheckTransform.position,
           m_groundCheckRadius,
           m_groundLayer
       );
    }

    public void HandleAllMovements()
    {
        HandleEnergyRegen();
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float horizontalInput = InputManager.Instance.horizontalInput;
        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;
        bool canSprint = m_currentEnergyValue > 0 && InputManager.Instance.isSprinting && isMoving;

        float targetSpeed;

        if (canSprint)
        {
            targetSpeed = m_sprintSpeed;
            m_currentEnergyValue -= m_sprintEnergyCost * Time.deltaTime;
        }
        else
        {
            targetSpeed = m_movementSpeed;
        }

        m_currentEnergyValue = Mathf.Max(m_currentEnergyValue, 0);

        Vector2 targetVelocity = new Vector2(horizontalInput * targetSpeed, m_playerRB.linearVelocity.y);
        m_playerRB.linearVelocity = Vector2.SmoothDamp(
            m_playerRB.linearVelocity,
            targetVelocity,
            ref m_velocityReference,
            m_acceleration
        );

        if (horizontalInput > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump()
    {
        if (m_isGrounded && InputManager.Instance.isJumped)
        {
            m_playerRB.linearVelocity = new Vector2(m_playerRB.linearVelocity.x, m_jumpForce);
        }
    }
    private void HandleEnergyRegen()
    {
        bool isActionPerforming = InputManager.Instance.isSprinting && Mathf.Abs(InputManager.Instance.horizontalInput) > 0.1f;

        if (!isActionPerforming && m_currentEnergyValue < m_maxEnergyValue)
        {
            m_currentEnergyValue += m_energyRegenRate * Time.deltaTime;
            m_currentEnergyValue = Mathf.Min(m_currentEnergyValue, m_maxEnergyValue);
        }
    }
    public bool IsGrounded() => m_isGrounded;
}