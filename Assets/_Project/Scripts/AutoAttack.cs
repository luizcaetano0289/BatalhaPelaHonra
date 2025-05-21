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
        lastAttackTime = Time.time; // Evita ataque precoce
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

        // 🔄 Remova a linha abaixo do Update!
        // lastAttackTime = Time.time; 
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

        lastAttackTime = Time.time; // ✅ Atualiza aqui, no momento exato do dano

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
        // NÃO reinicia cooldown aqui — espera tempo anterior finalizar
    }

    public void StopAutoAttack()
    {
        currentTarget = null;
        IsAutoAttacking = false;
        autoAttackActive = false;

        if (animator != null)
            animator.ResetTrigger("Attack");
    }

    public void TransferAutoAttackToNewTarget(GameObject newTarget)
    {
        if (newTarget == null || !newTarget.activeInHierarchy)
            return;

        float distance = Vector3.Distance(transform.position, newTarget.transform.position);

        // Só atualiza o alvo, sem resetar o cooldown
        currentTarget = newTarget;

        Debug.Log($"[AutoAttack] Transferido para novo alvo: {newTarget.name}");

        // Se estiver no alcance, continua normalmente
        if (distance <= attackRange)
        {
            // Nada muda no tempo do ataque, só aguardamos o próximo ciclo
        }
    }

}