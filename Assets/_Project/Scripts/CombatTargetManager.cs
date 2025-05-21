using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CombatTargetManager : MonoBehaviour
{
    [Header("Configurações")]
    public float tabTargetRange = 40f;
    public LayerMask enemyLayer;

    [Header("Referências")]
    public Transform playerTransform;
    public AutoAttack autoAttack;
    public TargetSelector targetSelector;

    private List<Targetable> tabTargets = new List<Targetable>();
    private int currentTabIndex = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            HandleTabTargeting();
        }
    }

    private void HandleTabTargeting()
    {
        Collider[] hits = Physics.OverlapSphere(playerTransform.position, tabTargetRange, enemyLayer);
        List<Targetable> enemies = hits
            .Select(col => col.GetComponent<Targetable>())
            .Where(t => t != null && (t.targetType == TargetType.EnemyNPC || t.targetType == TargetType.EnemyPlayer))
            .OrderBy(t => Vector3.Distance(playerTransform.position, t.transform.position))
            .ToList();

        if (enemies.Count == 0)
        {
            return;
        }

        // Se o alvo atual foi selecionado manualmente, sincroniza o índice
        Transform current = targetSelector.GetCurrentTarget();
        if (current != null)
        {
            int foundIndex = enemies.FindIndex(e => e.transform == current);
            if (foundIndex >= 0)
            {
                currentTabIndex = foundIndex;
            }
        }

        // Avança para o próximo alvo
        currentTabIndex++;
        if (currentTabIndex >= enemies.Count)
        {
            currentTabIndex = 0;
        }

        var newTarget = enemies[currentTabIndex];
        targetSelector.SetTarget(newTarget.transform);

        if (autoAttack.IsAutoAttacking)
        {
            autoAttack.StartAutoAttack(newTarget.gameObject);
        }
    }

    public void ResetTabIndex()
    {
        currentTabIndex = -1;
    }

    public void ClearTarget()
    {
        targetSelector.ClearTarget();
        autoAttack.StopAutoAttack();
        ResetTabIndex();
    }
}