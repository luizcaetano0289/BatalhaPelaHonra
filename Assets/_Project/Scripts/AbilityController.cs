using UnityEngine;

public class AbilityController : MonoBehaviour
{
    public GameObject player;
    public EnemySelector enemySelector;
    public AbilitySlot fireballSlot;

    public void CastFireball()
    {
        GameObject target = enemySelector.GetSelectedTarget();

        if (player != null && target != null && fireballSlot != null)
        {
            fireballSlot.Trigger(player, target);
        }
        else
        {
            Debug.LogWarning("Fireball n�o foi lan�ada: player, target ou slot est� nulo.");
        }
    }
}
