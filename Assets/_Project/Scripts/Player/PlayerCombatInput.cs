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
            targetSelector.ResetTabIndex(); // ✅ Agora corretamente no TargetSelector
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
                targetSelector.ResetTabIndex(); // ✅ Corrigido aqui também
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
        autoAttack.SetTarget(newTarget);

        // Se já estiver atacando, continue o ataque no novo alvo
        if (autoAttack.autoAttackActive && newTarget != null)
        {
            autoAttack.TransferAutoAttackToNewTarget(newTarget);
        }
    }

}
