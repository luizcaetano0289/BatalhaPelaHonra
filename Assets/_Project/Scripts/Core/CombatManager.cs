using UnityEngine;

public class CombatManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float maxAngleToAttack = 60f;

    public bool CanAttackCurrentTarget()
    {
        var target = GameManager.Instance.TargetSelector.GetCurrentTarget();
        if (target == null) return false;

        Vector3 directionToTarget = target.transform.position - playerTransform.position;
        float distance = directionToTarget.magnitude;
        directionToTarget.y = 0;

        if (distance > attackRange) return false;

        float angle = Vector3.Angle(playerTransform.forward, directionToTarget.normalized);
        if (angle > maxAngleToAttack) return false;

        return true;
    }
}
