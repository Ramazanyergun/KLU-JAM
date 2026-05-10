using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class Healthbar : MonoBehaviour
{
    public Slider Slider;
    public Gradient gradient;
    public Image fill;
     [SerializeField] private Health m_healthSystem;
    private PlayerHealth m_playerHealth;
    private PlayerMovement m_playerMovement;

    private void Awake()
    {
        if (m_healthSystem != null)
            m_playerHealth = m_healthSystem.GetComponent<PlayerHealth>();
        m_playerMovement = FindFirstObjectByType<PlayerMovement>();
    }
   
    private void OnEnable()
    {
        if (m_healthSystem != null)
            m_healthSystem.OnHealthDecreased += SetHealth;

 
        if (m_playerHealth != null)
            m_playerHealth.OnHealthIncreased += SetHealth;
    }

    private void OnDisable()
    {
        if (m_healthSystem != null)
            m_healthSystem.OnHealthDecreased -= SetHealth;
         if (m_playerHealth != null)
            m_playerHealth.OnHealthIncreased -= SetHealth;
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