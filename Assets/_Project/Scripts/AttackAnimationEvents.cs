using UnityEngine;

public class AttackAnimationEvents : MonoBehaviour
{
    [SerializeField] private AutoAttack autoAttack;

    public void ApplyAttackDamage()
    {
        if (autoAttack != null)
        {
            autoAttack.ApplyAttackDamage();
        }
    }
}
