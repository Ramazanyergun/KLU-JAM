using System;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private EnemyMovement m_movement;
    private EnemyHealth m_health;
    private EnemyCombat m_combat;
    private Animator m_animator;
    private int m_attackHash = Animator.StringToHash("Attack");
    private int m_takeDamageHash = Animator.StringToHash("TakeDamage");
    private int m_deathHash = Animator.StringToHash("Death");
    private int m_movingHash = Animator.StringToHash("IsMoving");

    void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_movement = GetComponent<EnemyMovement>();
        m_health = GetComponent<EnemyHealth>();
        m_combat = GetComponent<EnemyCombat>();

    }


    void OnEnable()
    {
        m_health.OnHealthChanged += TriggerTakeDamageAnimation;
        m_health.OnEnemyDeath += TriggerDeathAnimation;
        m_combat.OnAttack += TriggerAttackAnimation;
        m_movement.OnMoveStateChanged += UpdateMovementAnimation;
    }

    private void UpdateMovementAnimation(bool isMoving)
    {
        m_animator.SetBool(m_movingHash, isMoving);
    }

    private void TriggerAttackAnimation()
    {
        m_animator.SetTrigger(m_attackHash);
    }

    private void TriggerDeathAnimation()
    {
        m_animator.SetTrigger(m_deathHash);
    }

    private void TriggerTakeDamageAnimation(float obj)
    {
        m_animator.SetTrigger(m_takeDamageHash);
    }

    void OnDisable()
    {
        m_health.OnHealthChanged -= TriggerTakeDamageAnimation;
        m_health.OnEnemyDeath -= TriggerDeathAnimation;
        m_combat.OnAttack -= TriggerAttackAnimation;
        m_movement.OnMoveStateChanged -= UpdateMovementAnimation;
    }
}
