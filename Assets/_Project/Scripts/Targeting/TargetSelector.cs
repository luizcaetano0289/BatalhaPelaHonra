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
            if (t != null && (t.targetType == TargetType.EnemyNPC || t.targetType == TargetType.EnemyPlayer))
            {
                enemies.Add(t);
            }
        }

        if (enemies.Count == 0) return;

        if (currentTarget == null || !enemies.Contains(currentTarget))
        {
            SetTarget(enemies[0].transform);
            return;
        }

        int currentIndex = enemies.IndexOf(currentTarget);
        int nextIndex = (currentIndex + 1) % enemies.Count;
        SetTarget(enemies[nextIndex].transform);
    }

    public void SetTarget(Transform newTarget)
    {
        Targetable target = newTarget.GetComponent<Targetable>();
        if (target == null) return;

        currentTarget = target;
        UpdateTargetIndicator();
    }

    private void UpdateTargetIndicator()
    {
        // Destroi indicador anterior
        if (currentIndicator != null)
            Destroy(currentIndicator);

        // Destroi HUD anterior
        if (currentFloatingHUD != null)
            Destroy(currentFloatingHUD);

        if (currentTarget == null) return;

        // Instancia círculo de seleção
        currentIndicator = Instantiate(indicatorPrefab);
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

        // Instancia HUD flutuante se tiver UnitStats
        UnitStats stats = currentTarget.GetComponent<UnitStats>();
        if (stats != null)
        {
            currentFloatingHUD = Instantiate(floatingHudPrefab);
            currentFloatingHUD.transform.SetParent(currentTarget.transform, false);
            currentFloatingHUD.transform.localPosition = new Vector3(0, 1.8f, 0);  // Aqui muda a posição do HUD de vida do alvo

            FloatingHUD hud = currentFloatingHUD.GetComponent<FloatingHUD>();
            hud.Initialize(stats);            
        }

    }

    public Transform GetCurrentTarget()
    {
        return currentTarget != null ? currentTarget.transform : null;
    }
}
