using UnityEngine;

public class PlayerCombatInput : MonoBehaviour
{
    [SerializeField] private TargetSelector targetSelector;
    [SerializeField] private CombatTargetManager targetManager;
    private AutoAttack autoAttack;
    private Transform playerTransform;

    void Start()
    {
        autoAttack = GetComponent<AutoAttack>();
        targetSelector = GetComponent<TargetSelector>();
        targetManager = FindObjectOfType<CombatTargetManager>();
        playerTransform = transform;

        if (targetSelector != null)
            targetSelector.OnTargetChanged += HandleTargetChanged;
    }

    void Update()
    {
        HandleLeftClickSelection();
        HandleRightClickAttack();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            autoAttack.StopAutoAttack();
            targetSelector.ClearTarget();
            targetManager.ResetTabIndex(); // Corrigido aqui
        }
    }

    private void HandleLeftClickSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

            bool hitSomething = false;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    hitSomething = true;
                    targetSelector.SetTarget(hit.transform);

                    if (autoAttack.autoAttackActive)
                        autoAttack.StartAutoAttack(hit.transform.gameObject);

                    break;
                }
            }

            if (!hitSomething)
            {
                targetSelector.ClearTarget();
                autoAttack.StopAutoAttack();
                targetManager.ResetTabIndex(); // Corrigido aqui
            }
        }
    }

    private void HandleRightClickAttack()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    GameObject enemy = hit.collider.gameObject;
                    targetSelector.SetTarget(enemy.transform);
                    autoAttack.StartAutoAttack(enemy);
                    break;
                }
            }
        }
    }

    private void HandleTargetChanged(GameObject newTarget)
    {
        autoAttack.StopAutoAttack();
        autoAttack.SetTarget(newTarget);
    }
}
