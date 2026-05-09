using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider Slider;
    public Image fill;
    public Gradient gradient;

    [SerializeField] private PlayerMovement m_movementSystem;

    private void OnEnable()
    {
        if (m_movementSystem != null)
            m_movementSystem.OnEnergyChanged += UpdateStaminaBar;
    }

    private void OnDisable()
    {
        if (m_movementSystem != null)
            m_movementSystem.OnEnergyChanged -= UpdateStaminaBar;
    }

    private void UpdateStaminaBar(float currentEnergy, float maxEnergy)
    {
        Slider.maxValue = maxEnergy;
        Slider.value = currentEnergy;

        if (fill != null && gradient != null)
        {
            fill.color = gradient.Evaluate(Slider.normalizedValue);
        }
    }
}
