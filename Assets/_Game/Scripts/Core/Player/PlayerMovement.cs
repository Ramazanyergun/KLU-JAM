using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public event Action<bool> OnSwapChanged;
    public event Action<int> OnMoveStateChanged;
    public event Action OnJump;
    public event Action<float, bool> OnAirUpdate;
    public event Action<float, float> OnEnergyChanged;
    [Header("Movement Settings")]
    [SerializeField] private float m_movementSpeed = 5f;
    [SerializeField] private float m_sprintSpeed = 8f;
    [SerializeField] private float m_acceleration = 0.1f;
    [SerializeField] private float m_maxEnergyValue = 100;
    [SerializeField] private float m_currentEnergyValue;
    [SerializeField] private float m_energyRegenRate = 10f;
    [SerializeField] private float m_sprintEnergyCost = 20f;

    public float CurrentEnergy => m_currentEnergyValue;
    [Header("Jump Settings")]
    [SerializeField] private float m_jumpForce = 10f;
    [SerializeField] private float m_jumpCost;

    [Header("Detection")]
    [SerializeField] private Transform m_groundCheckTransform;
    [SerializeField] private float m_groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask m_groundLayer;

    [Header("Glitch Settings")]
    [SerializeField] private bool m_reverseControls;
    [SerializeField] private bool m_swapHealthAndEnergy;
    public bool IsSwapActive => m_swapHealthAndEnergy;

    private Rigidbody2D m_playerRB;
    private PlayerHealth m_playerHealth;
    private Vector2 m_velocityReference = Vector2.zero;
    private bool m_isGrounded;

    void Awake()
    {
        m_playerRB = GetComponent<Rigidbody2D>();
        m_playerHealth = GetComponent<PlayerHealth>();
        m_currentEnergyValue = m_maxEnergyValue;
    }

    void FixedUpdate()
    {
        m_isGrounded = Physics2D.OverlapCircle(m_groundCheckTransform.position, m_groundCheckRadius, m_groundLayer);

        OnAirUpdate?.Invoke(m_playerRB.linearVelocity.y, m_isGrounded);
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

        if (m_reverseControls)
        {
            horizontalInput *= -1;
        }

        bool isMoving = Mathf.Abs(horizontalInput) > 0.1f;

        bool hasResource =
            m_swapHealthAndEnergy
            ? m_playerHealth.CurrentHealth > 0
            : m_currentEnergyValue > 0;

        bool canSprint =
            hasResource &&
            InputManager.Instance.isSprinting &&
            isMoving;
        float targetSpeed = canSprint ? m_sprintSpeed : m_movementSpeed;

        if (canSprint)
        {
            bool canConsume =
                ConsumeResource(m_sprintEnergyCost * Time.deltaTime);

            if (!canConsume)
            {
                canSprint = false;
            }
            OnEnergyChanged?.Invoke(m_currentEnergyValue, m_maxEnergyValue);
        }
        m_currentEnergyValue = Mathf.Max(m_currentEnergyValue, 0);


        Vector2 targetVelocity = new Vector2(horizontalInput * targetSpeed, m_playerRB.linearVelocity.y);
        m_playerRB.linearVelocity = Vector2.SmoothDamp(m_playerRB.linearVelocity, targetVelocity, ref m_velocityReference, m_acceleration);


        int state = isMoving ? 1 : 0;
        OnMoveStateChanged?.Invoke(state);

        if (horizontalInput > 0.1f) transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.1f) transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJump()
    {
        if (m_isGrounded && InputManager.Instance.isJumped)
        {
            bool canJump = ConsumeResource(m_jumpCost);

            if (!canJump) return;

            m_playerRB.linearVelocity = new Vector2(m_playerRB.linearVelocity.x, m_jumpForce);

            OnJump?.Invoke();
        }
    }
    public void SetSwapHealthAndEnergy(bool value)
    {
        m_swapHealthAndEnergy = value;
        OnSwapChanged?.Invoke(value);
    }

    private bool ConsumeResource(float amount)
    {
        if (m_swapHealthAndEnergy)
        {
            if (m_playerHealth.CurrentHealth <= amount)
            {
                m_playerHealth.TakeDamage(amount);
                CheckDeath();
                return true;
            }

            m_playerHealth.TakeDamage(amount);
            CheckDeath();
            return true;
        }
        else
        {
            if (m_currentEnergyValue <= amount)
            {
                m_currentEnergyValue -= amount;
                CheckDeath();
                return false;
            }

            m_currentEnergyValue -= amount;
            CheckDeath();
            return true;
        }
    }

    public void UseEnergy(float amount)
    {
        m_currentEnergyValue -= amount;
        m_currentEnergyValue = Mathf.Clamp(m_currentEnergyValue, 0, m_maxEnergyValue);
    }

    private void HandleEnergyRegen()
    {
        bool isActionPerforming =
            InputManager.Instance.isSprinting &&
            Mathf.Abs(InputManager.Instance.horizontalInput) > 0.1f;

        if (isActionPerforming)
            return;

        if (!m_swapHealthAndEnergy)
        {
            if (m_currentEnergyValue < m_maxEnergyValue)
            {
                m_currentEnergyValue +=
                    m_energyRegenRate * Time.deltaTime;

                m_currentEnergyValue =
                    Mathf.Min(m_currentEnergyValue, m_maxEnergyValue);

                OnEnergyChanged?.Invoke(
                    m_currentEnergyValue,
                    m_maxEnergyValue);
            }
        }
        else
        {
            if (m_playerHealth.CurrentHealth < m_playerHealth.MaxHealth)
            {
                m_playerHealth.Heal(
                    m_energyRegenRate * Time.deltaTime);
            }
        }
    }

    public void TakeResourceDamage(float amount)
    {
        Debug.Log("SWAP STATE: " + m_swapHealthAndEnergy);

        if (m_swapHealthAndEnergy)
        {
            Debug.Log("DAMAGE TO ENERGY");
            m_currentEnergyValue -= amount;
            m_currentEnergyValue =
                Mathf.Clamp(m_currentEnergyValue, 0, m_maxEnergyValue);

            OnEnergyChanged?.Invoke(m_currentEnergyValue, m_maxEnergyValue);

        }
        else
        {
            Debug.Log("DAMAGE TO HEALTH");
            m_playerHealth.TakeDamage(amount);
        }
    }
    public void CheckDeath()
    {
        if (m_swapHealthAndEnergy)
        {
            if (m_playerHealth.CurrentHealth <= 0)
                m_playerHealth.Die();
        }
        else
        {
            if (m_currentEnergyValue <= 0)
                m_playerHealth.Die();
        }
    }
    public bool IsDead()
    {
        if (m_swapHealthAndEnergy)
            return m_playerHealth.CurrentHealth <= 0;
        else
            return m_currentEnergyValue <= 0;
    }

    public bool ConsumeDefense(float amount)
    {
        return ConsumeResource(amount);
    }

    public void SetReverseControls(bool value)
    {
        m_reverseControls = value;
    }


}