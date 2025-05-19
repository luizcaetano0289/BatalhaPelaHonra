using UnityEngine;
using System.Collections.Generic;

public class TargetSelector : MonoBehaviour
{
    [Header("Seleção por clique")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float maxSelectionDistance = 100f;

    [Header("Seleção com Tab")]
    [SerializeField] private float detectionRadius = 30f;

    [Header("Prefabs")]
    [SerializeField] private GameObject indicatorPrefab; // Círculo no chão
    [SerializeField] private GameObject floatingHudPrefab; // HUD flutuante

    private GameObject currentIndicator;
    private GameObject currentFloatingHUD;
    private Targetable currentTarget;

    public System.Action<GameObject> OnTargetChanged;
    private int currentTargetIndex = -1;

    void Update()
    {
        HandleClickSelection();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleEnemyTarget();
        }
    }

    private void HandleClickSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, maxSelectionDistance, targetLayer))
            {
                Targetable target = hit.collider.GetComponent<Targetable>();
                if (target != null)
                {
                    SetTarget(target.transform);
                }
            }
        }
    }

    private void CycleEnemyTarget()
    {
        Collider[] nearby = Physics.OverlapSphere(transform.position, detectionRadius);
        List<Targetable> enemies = new List<Targetable>();

        foreach (Collider col in nearby)
        {
            Targetable t = col.GetComponent<Targetable>();
            if (t != null &&
                (t.targetType == TargetType.EnemyNPC || t.targetType == TargetType.EnemyPlayer))
            {
                enemies.Add(t);
            }
        }

        if (enemies.Count == 0)
            return;

        // Garante consistência da lista
        enemies.Sort((a, b) => a.name.CompareTo(b.name));

        // Atualiza índice
        currentTargetIndex = (currentTargetIndex + 1) % enemies.Count;

        SetTarget(enemies[currentTargetIndex].transform);
    }

    public void SetTarget(Transform newTarget)
    {
        Targetable target = newTarget.GetComponent<Targetable>();
        if (target == null)
            return;

        // Atualiza o novo alvo mesmo que seja igual, para forçar a limpeza visual
        currentTarget = target;
        UpdateTargetIndicator();
        OnTargetChanged?.Invoke(target.gameObject);

    }

    private void UpdateTargetIndicator()
    {
        // 🧹 Destroi todos os HUDs e círculos de todos os inimigos ativos na cena
        foreach (var t in FindObjectsOfType<Targetable>())
        {
            foreach (Transform child in t.transform)
            {
                if (child.name.Contains("Indicator") || child.GetComponent<FloatingHUD>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }

        currentIndicator = null;
        currentFloatingHUD = null;

        if (currentTarget == null)
            return;

        // 🎯 Círculo de seleção
        currentIndicator = Instantiate(indicatorPrefab);
        currentIndicator.name = "Indicator"; // facilita identificação
        currentIndicator.transform.SetParent(currentTarget.transform, false);

        // Ajusta posição no chão
        Collider col = currentTarget.GetComponent<Collider>();
        if (col != null)
        {
            float yOffset = -col.bounds.extents.y + 0.02f;
            currentIndicator.transform.localPosition = new Vector3(0, yOffset, 0);
        }

        // Define cor do indicador
        Renderer r = currentIndicator.GetComponentInChildren<Renderer>();
        if (r != null)
        {
            switch (currentTarget.targetType)
            {
                case TargetType.AllyNPC:
                case TargetType.AllyPlayer:
                    r.material = Resources.Load<Material>("Mat_Target_Ally");
                    break;
                case TargetType.EnemyNPC:
                case TargetType.EnemyPlayer:
                    r.material = Resources.Load<Material>("Mat_Target_Enemy");
                    break;
            }
        }

        // 🎯 HUD flutuante
        UnitStats stats = currentTarget.GetComponent<UnitStats>();
        if (stats != null)
        {
            currentFloatingHUD = Instantiate(floatingHudPrefab);
            currentFloatingHUD.name = "FloatingHUD";
            currentFloatingHUD.transform.SetParent(currentTarget.transform, false);
            currentFloatingHUD.transform.localPosition = new Vector3(0, 1.8f, 0);

            FloatingHUD hud = currentFloatingHUD.GetComponent<FloatingHUD>();
            hud.Initialize(stats);
        }
    }

    public Transform GetCurrentTarget()
    {
        return currentTarget != null ? currentTarget.transform : null;
    }
}
