using UnityEngine;

public class EnemyAnimationBridge : MonoBehaviour
{
    private EnemyCombat m_enemyCombat;
    void Awake()
    {
        m_enemyCombat = GetComponentInParent<EnemyCombat>();
    }


    public void ApplyDamage()
    {
        m_enemyCombat.AnimationTriggerStep();
    }
}
