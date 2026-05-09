using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private PlayerMovement m_movement;
    private PlayerCombat m_combat;
    private Animator m_animator;

    private int m_animStateHash = Animator.StringToHash("AnimState");
    private int m_groundedHash = Animator.StringToHash("Grounded");
    private int m_airSpeedYHash = Animator.StringToHash("AirSpeedY");
    private int m_jumpHash = Animator.StringToHash("Jump");
    private int m_blockHash = Animator.StringToHash("Block");
    private int m_attackHash = Animator.StringToHash("Attack");

    void Awake()
    {
        m_animator = GetComponentInChildren<Animator>();
        m_movement = GetComponent<PlayerMovement>();
        m_combat = GetComponent<PlayerCombat>();
    }

    void OnEnable()
    {
        if (m_movement != null)
        {
            m_movement.OnMoveStateChanged += UpdateMoveAnimation;
            m_movement.OnJump += TriggerJumpAnimation;
            m_movement.OnAirUpdate += UpdateAirStatus;
            m_combat.OnAttack += PlayAttackAnim;
            m_combat.OnDefenseStatusChanged += UpdateDefenseAnim;
        }
    }

    void OnDisable()
    {
        if (m_movement != null)
        {
            m_movement.OnMoveStateChanged -= UpdateMoveAnimation;
            m_movement.OnJump -= TriggerJumpAnimation;
            m_movement.OnAirUpdate -= UpdateAirStatus;
            m_combat.OnAttack -= PlayAttackAnim;
            m_combat.OnDefenseStatusChanged -= UpdateDefenseAnim;
        }
    }

    private void UpdateMoveAnimation(int state)
    {
        m_animator.SetInteger(m_animStateHash, state);
    }

    private void TriggerJumpAnimation()
    {
        m_animator.SetTrigger(m_jumpHash);
    }

    private void UpdateAirStatus(float verticalSpeed, bool grounded)
    {
        m_animator.SetFloat(m_airSpeedYHash, verticalSpeed);
        m_animator.SetBool(m_groundedHash, grounded);
    }
    private void PlayAttackAnim()
    {
        m_animator.SetTrigger(m_attackHash);
    }

    private void UpdateDefenseAnim(bool isDefending)
    {
        m_animator.SetBool(m_blockHash, isDefending);
    }

}