using UnityEngine;

public class CombatTargetManager : MonoBehaviour
{
    [Header("Referências")]
    public Transform playerTransform;
    public AutoAttack autoAttack;
    public TargetSelector targetSelector;

    void Start()
    {
        if (autoAttack == null)
            autoAttack = FindObjectOfType<AutoAttack>();

        if (targetSelector == null)
            targetSelector = FindObjectOfType<TargetSelector>();
    }

    /// <summary>
    /// Ataca o alvo com clique direito do mouse (deve ser chamado pelo PlayerCombatInput)
    /// </summary>
    public void HandleRightClick(RaycastHit hit)
    {
        if (hit.collider == null || hit.collider.GetComponent<Targetable>() == null)
            return;

        var target = hit.collider.GetComponent<Targetable>();
        if (target == null)
            return;

        targetSelector.SetTarget(target.transform);
        autoAttack.StartAutoAttack(target.gameObject);
    }

    /// <summary>
    /// Cancela ataque e limpa o alvo atual
    /// </summary>
    public void ClearTarget()
    {
        if (targetSelector != null)
            targetSelector.ClearTarget();

        if (autoAttack != null)
            autoAttack.StopAutoAttack();
    }
}
