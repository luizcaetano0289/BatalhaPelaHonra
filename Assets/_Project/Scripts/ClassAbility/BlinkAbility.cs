using UnityEngine;

public class BlinkAbility : AbilityBase
{
    [Header("Parâmetros de Teleporte")]
    public float blinkDistance = 7f;
    public LayerMask obstacleMask;

    public override void Execute(GameObject caster, GameObject target = null)
    {
        Vector3 direction = caster.transform.forward;
        Vector3 origin = caster.transform.position;
        Vector3 destination = origin + direction * blinkDistance;

        // Verifica colisões no caminho
        if (Physics.Raycast(origin, direction, out RaycastHit hit, blinkDistance, obstacleMask))
        {
            destination = hit.point - direction * 1f; // para não colidir dentro da parede
        }

        // Move o jogador
        caster.transform.position = destination;
        Debug.Log($"{abilityName} teletransportou {caster.name} para {destination}");
    }
}
