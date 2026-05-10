using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider m_slider;
    [SerializeField] private Gradient m_gradient;
    [SerializeField] private Image m_fill;
    [SerializeField] private TextMeshProUGUI m_stateText;

    private PlayerMovement m_playerMovement;
    private PlayerHealth m_playerHealth;

    private void Awake()
    {
        m_playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (m_playerMovement != null)
        {
            m_playerHealth =
                m_playerMovement.GetComponent<PlayerHealth>();
        }
    }

    private void OnEnable()
    {
        if (m_playerMovement != null)
        {
            m_playerMovement.OnEnergyChanged += UpdateEnergyBar;
            m_playerMovement.OnSwapChanged += UpdateStateText;
        }

        if (m_playerHealth != null)
        {
            m_playerHealth.OnHealthDecreased += UpdateHealthBar;
            m_playerHealth.OnHealthIncreased += UpdateHealthBar;
        }
    }

    private void Start()
    {
        if (m_playerMovement == null) return;

        UpdateStateText(m_playerMovement.IsSwapActive);

        if (m_playerMovement.IsSwapActive)
        {
            SetBar(
                m_playerHealth.CurrentHealth,
                m_playerHealth.MaxHealth);
        }
        else
        {
            SetBar(
                m_playerMovement.CurrentEnergy,
                100f);
        }
    }

    private void OnDisable()
    {
        if (m_playerMovement != null)
        {
            m_playerMovement.OnEnergyChanged -= UpdateEnergyBar;
            m_playerMovement.OnSwapChanged -= UpdateStateText;
        }

        if (m_playerHealth != null)
        {
            m_playerHealth.OnHealthDecreased -= UpdateHealthBar;
            m_playerHealth.OnHealthIncreased -= UpdateHealthBar;
        }
    }

    private void UpdateEnergyBar(float current, float max)
    {
        if (m_playerMovement.IsSwapActive) return;

        SetBar(current, max);
    }

    private void UpdateHealthBar(float current)
    {
        if (!m_playerMovement.IsSwapActive) return;

        SetBar(current, m_playerHealth.MaxHealth);
    }

    private void SetBar(float current, float max)
    {
        m_slider.maxValue = max;
        m_slider.value = current;

        m_fill.color =
            m_gradient.Evaluate(m_slider.normalizedValue);
    }

    private void UpdateStateText(bool isSwapActive)
    {
        if (m_stateText == null) return;

        m_stateText.text =
            isSwapActive
            ? "HEALTH MODE"
            : "ENERGY MODE";
    }
}