using System;
using System.Collections;
using UnityEditor.Media;
using UnityEngine;

public class PlayerStatusEffects : MonoBehaviour
{
    private PlayerMovement m_movement;
    private PlayerHealth m_health;
    private InputManager m_inputManager;


    private bool m_reverseControls;
    private bool m_swapHealthEnergy;

    public bool ReverseControls => m_reverseControls;
    public bool SwapHealthEnergy => m_swapHealthEnergy;


    private void Awake()
    {
        m_movement = GetComponent<PlayerMovement>();
        m_health = GetComponent<PlayerHealth>();
        m_inputManager = InputManager.Instance;
    }


    public void ApplyGlitch(EffectTypes effectType, float duration)
    {
        StartCoroutine(GlitchCoroutine(effectType, duration));
    }

    private IEnumerator GlitchCoroutine(EffectTypes effectType, float duration)
    {


        EnableGlitch(effectType);
        yield return new WaitForSeconds(duration);

        DisableGlitch(effectType);
    }


    private void EnableGlitch(EffectTypes effectType)
    {
        switch (effectType)
        {
            case EffectTypes.ReverseControls:
                m_reverseControls = true;
                break;

            case EffectTypes.SwapHealthAndEnergy:
                m_swapHealthEnergy = true;
                break;
        }
    }

    private void DisableGlitch(EffectTypes effectType)
    {

        switch (effectType)
        {
            case EffectTypes.ReverseControls:
                m_reverseControls = false;
                break;
                     
            case EffectTypes.SwapHealthAndEnergy:
                m_swapHealthEnergy = false;
                break;
        }
    }


}
