using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider Slider;
    public Gradient gradient;
    public Image fill;

    [SerializeField] private Health m_healthSystem;



    private void OnEnable()
    {
        if (m_healthSystem != null)
            m_healthSystem.OnHealthChanged += SetHealth;
    }

    private void OnDisable()
    {
        if (m_healthSystem != null)
            m_healthSystem.OnHealthChanged -= SetHealth;
    }

    public void SetMaxHealth(float health)
    {
        Slider.maxValue = health;
        Slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(float health)
    {
        Slider.value = health;
        fill.color = gradient.Evaluate(Slider.normalizedValue);
    }
}