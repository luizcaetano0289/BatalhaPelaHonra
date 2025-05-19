using UnityEngine;

public class WeaponSwing : MonoBehaviour
{
    [Header("Swing Settings")]
    public float swingSpeed = 2.0f; // tempo entre ataques brancos (em segundos)
    public float swingCooldown = 0f; // contador regressivo

    private Animator animator;
    private GameObject currentTarget;

    public int damage = 20;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        ResetSwing();
    }

    void Update()
    {
        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            return;
        }

        swingCooldown -= Time.deltaTime;

        if (swingCooldown <= 0f)
        {
            PerformSwing();
            ResetSwing();
        }
    }

    private void ResetSwing()
    {
        swingCooldown = swingSpeed;
    }

    private void PerformSwing()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // AnimationEvent chamará ApplySwingDamage
        }
    }

    public void ApplySwingDamage()
    {
        if (currentTarget != null && currentTarget.TryGetComponent(out UnitStats stats))
        {
            stats.TakeDamage(damage);
            Debug.Log($"[Swing] Dano branco aplicado: {damage}");
        }
    }

    public void SetTarget(GameObject target)
    {
        currentTarget = target;
    }

    public void StopSwing()
    {
        currentTarget = null;
        swingCooldown = 0f;
    }
}
