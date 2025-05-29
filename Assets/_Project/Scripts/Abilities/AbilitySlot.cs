using UnityEngine;

public class AbilitySlot : MonoBehaviour
{
    public AbilityBase ability;

    public void Trigger(GameObject caster, GameObject target)
    {
        if (ability == null)
        {
            Debug.LogWarning("Nenhuma habilidade atribuída a este slot.");
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Nenhum alvo selecionado.");
            return;
        }

        float distance = Vector3.Distance(caster.transform.position, target.transform.position);
        if (distance > ability.range)
        {
            Debug.Log("Alvo fora do alcance.");
            return;
        }

        AbilityController.Instance.TryCastAbility(ability, caster, target);
    }

    public void TriggerFromPlayer()
    {
        GameObject caster = AbilityController.Instance.player;
        GameObject target = TargetSelector.Instance.GetCurrentTarget()?.gameObject;

        if (caster == null)
        {
            Debug.LogWarning("Player (caster) não está atribuído no AbilityController.");
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Nenhum alvo selecionado.");
            return;
        }

        Trigger(caster, target);
    }

}
