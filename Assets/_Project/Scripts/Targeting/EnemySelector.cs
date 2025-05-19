using UnityEngine;
using System.Collections.Generic;

public class EnemySelector : MonoBehaviour
{
    public GameObject currentTarget;
    public float selectionRadius = 30f;
    private List<GameObject> nearbyEnemies = new List<GameObject>();
    private int targetIndex = 0;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Clique esquerdo
        {
            SelectTargetWithMouse();
        }

        if (Input.GetKeyDown(KeyCode.Tab)) // Tecla Tab
        {
            CycleThroughEnemies();
        }
    }

    void SelectTargetWithMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Enemy"))
            {
                SetTarget(hitObject);
            }
        }
    }

    void CycleThroughEnemies()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, selectionRadius);

        nearbyEnemies.Clear();
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                nearbyEnemies.Add(hit.gameObject);
            }
        }

        if (nearbyEnemies.Count == 0) return;

        targetIndex++;
        if (targetIndex >= nearbyEnemies.Count) targetIndex = 0;

        SetTarget(nearbyEnemies[targetIndex]);
    }

    public void SetTarget(GameObject target)
    {
        currentTarget = target;
        Debug.Log("Alvo selecionado: " + target.name);
        // Aqui você pode adicionar um efeito visual de seleção, se quiser
    }

    public GameObject GetSelectedTarget()
    {
        return currentTarget;
    }
}
