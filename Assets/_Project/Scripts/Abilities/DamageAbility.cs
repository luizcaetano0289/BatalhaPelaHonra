using UnityEngine;

public class DamageAbility : AbilityBase
{
    [Header("Par�metros de Dano")]
    public int damageAmount = 50;

    public override void Execute(GameObject caster, GameObject target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out UnitStats stats))
        {
            stats.TakeDamage(damageAmount);
            Debug.Log($"{abilityName} causou {damageAmount} de dano a {target.name}");
        }
        else
        {
            Debug.LogWarning("O alvo n�o possui o componente UnitStats.");
        }
    }
}
