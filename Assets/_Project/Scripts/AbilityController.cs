using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public GameObject player;
    public TargetSelector targetSelector;
    public AbilitySlot fireballSlot;

    public void CastFireball()
    {
        Transform targetTransform = targetSelector.GetCurrentTarget();
        GameObject target = targetTransform != null ? targetTransform.gameObject : null;

        if (player != null && target != null && fireballSlot != null)
        {
            fireballSlot.Trigger(player, target);
        }
        else
        {
            Debug.LogWarning("Fireball não foi lançada: player, target ou slot está nulo.");
        }
    }
}