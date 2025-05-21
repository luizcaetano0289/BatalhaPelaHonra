﻿using UnityEngine;
using System.Collections.Generic;

public class TargetSelector : MonoBehaviour
{
    [Header("Seleção por clique")]
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float maxSelectionDistance = 100f;

    [Header("Seleção com Tab")]
    [SerializeField] private float detectionRadius = 30f;

    [Header("Prefabs")]
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private GameObject floatingHudPrefab;

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
        Collider[] nearby = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);
        List<Targetable> enemies = new List<Targetable>();

        foreach (Collider col in nearby)
        {
            Targetable t = col.GetComponent<Targetable>();
            if (t != null && (t.targetType == TargetType.EnemyNPC || t.targetType == TargetType.EnemyPlayer))
            {
                enemies.Add(t);
            }
        }

        if (enemies.Count == 0)
            return;

        // Ordena pela distância ao player
        enemies.Sort((a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        // Atualiza índice com base no alvo atual
        var current = GetCurrentTarget();
        if (current != null)
        {
            currentTargetIndex = enemies.FindIndex(e => e.transform == current);
        }
        else
        {
            currentTargetIndex = -1;
        }

        currentTargetIndex = (currentTargetIndex + 1) % enemies.Count;

        SetTarget(enemies[currentTargetIndex].transform);
    }

    public void SetTarget(Transform newTarget)
    {
        Targetable target = newTarget.GetComponent<Targetable>();
        if (target == null)
            return;

        currentTarget = target;
        UpdateTargetIndicator();
        OnTargetChanged?.Invoke(target.gameObject);

        Debug.Log("[Selecionado] " + target.name); // ← Mantenha esse
    }


    private void UpdateTargetIndicator()
    {
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

        currentIndicator = Instantiate(indicatorPrefab);
        currentIndicator.name = "Indicator";
        currentIndicator.transform.SetParent(currentTarget.transform, false);

        Collider col = currentTarget.GetComponent<Collider>();
        if (col != null)
        {
            float yOffset = -col.bounds.extents.y + 0.02f;
            currentIndicator.transform.localPosition = new Vector3(0, yOffset, 0);
        }

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

    public void ClearTarget()
    {
        currentTarget = null;
        currentTargetIndex = -1; // reseta a rotação do tab
        if (currentFloatingHUD != null) Destroy(currentFloatingHUD);
        if (currentIndicator != null) Destroy(currentIndicator);
    }
}
