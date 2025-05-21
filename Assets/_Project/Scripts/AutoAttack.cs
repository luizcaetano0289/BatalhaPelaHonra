using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public bool autoAttackActive { get; private set; } = false;
    public bool IsAutoAttacking { get; private set; }

    [Header("Configuração do Ataque")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 20;

    private float lastAttackTime;
    private GameObject currentTarget;

    private Animator animator;
    private SwingTimerUI swingUI;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lastAttackTime = Time.time - attackCooldown;
        animator.ResetTrigger("Attack");
        swingUI = FindObjectOfType<SwingTimerUI>();
    }

    void Update()
    {
        if (!IsAutoAttacking)
            return;

        if (currentTarget == null || !currentTarget.activeInHierarchy)
        {
            StopAutoAttack();
            return;
        }

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

        if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    public void SetTarget(GameObject target)
    {
        currentTarget = target;
    }

    private void Attack()
    {
        if (currentTarget == null || !currentTarget.activeInHierarchy)
            return;

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance > attackRange)
        {
            Debug.LogWarning("Distância insuficiente para iniciar ataque.");
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("Attack");
            Debug.Log("Animação de ataque iniciada.");
        }
    }

    public void ApplyAttackDamage()
    {
        if (currentTarget == null || !currentTarget.activeInHierarchy)
            return;

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance > attackRange)
        {
            Debug.LogWarning("Alvo fora do alcance no frame do impacto — dano cancelado.");
            return;
        }

        if (currentTarget.TryGetComponent(out UnitStats stats))
        {
            stats.TakeDamage(damage);
            Debug.Log("Dano do ataque aplicado via evento de animação.");

            if (swingUI != null)
                swingUI.StartSwing(attackCooldown);
        }
    }

    public void StartAutoAttack(GameObject target)
    {
        if (target == null || !target.activeInHierarchy)
            return;

        currentTarget = target;
        IsAutoAttacking = true;
        autoAttackActive = true;

        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance <= attackRange)
        {
            lastAttackTime = Time.time - attackCooldown; // permite atacar imediatamente
        }
        else
        {
            lastAttackTime = Time.time;
        }
    }

    public void StopAutoAttack()
    {
        currentTarget = null;
        IsAutoAttacking = false;
        autoAttackActive = false;

        if (animator != null)
            animator.ResetTrigger("Attack");
    }
}