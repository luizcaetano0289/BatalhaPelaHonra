using UnityEngine;

public class AbilitySlot : MonoBehaviour
{
    public AbilityBase ability;

    public void Trigger(GameObject caster, GameObject target)
    {
        if (ability == null || target == null) return;

        float distance = Vector3.Distance(caster.transform.position, target.transform.position);
        if (distance > ability.range)
        {
            Debug.Log("Alvo fora do alcance.");
            return;
        }

        ability.Execute(caster, target);
    }
}
