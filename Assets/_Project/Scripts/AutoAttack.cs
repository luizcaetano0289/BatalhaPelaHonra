using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public bool autoAttackActive { get; private set; } = false;

    [Header("Configuração do Ataque")]
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage = 20;

    private float lastAttackTime;
    private GameObject currentTarget;

    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        lastAttackTime = Time.time - attackCooldown; // força o primeiro ataque a sair instantâneo
        animator.ResetTrigger("Attack");
    }

    void Update()
    {
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
        }
    }

    public void StartAutoAttack(GameObject target)
    {
        SetTarget(target);

        float distance = Vector3.Distance(transform.position, target.transform.position);

        // Só começa a atacar se estiver no alcance
        if (distance <= attackRange)
        {
            lastAttackTime = Time.time;
        }
        else
        {
            lastAttackTime = Time.time - attackCooldown + 0.1f; // espera o player se aproximar
        }
    }


    public void StopAutoAttack()
    {
        currentTarget = null;
        autoAttackActive = false;

        if (animator != null)
            animator.ResetTrigger("Attack");
    }

}
