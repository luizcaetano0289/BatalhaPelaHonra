using UnityEngine;

public class PlayerCombatInput : MonoBehaviour
{
    private AutoAttack autoAttack;

    [SerializeField] private TargetSelector targetSelector;

    void Start()
    {
        autoAttack = GetComponent<AutoAttack>();
        targetSelector = GetComponent<TargetSelector>(); // ← Resolve automaticamente
        targetSelector.OnTargetChanged += HandleTargetChanged;

    }

    void Update()
    {
        HandleLeftClickSelection();
        HandleRightClickAttack();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            autoAttack.StopAutoAttack();
        }
    }

    private void HandleLeftClickSelection()
    {
        if (Input.GetMouseButtonDown(0)) // Botão esquerdo
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Transform enemy = hit.transform;

                    if (targetSelector != null)
                        targetSelector.SetTarget(enemy);

                    // Se estiver atacando, já troca o alvo e continua o ataque
                    if (autoAttack.autoAttackActive)
                    {
                        autoAttack.StartAutoAttack(enemy.gameObject);
                    }

                    break; // para no primeiro inimigo encontrado
                }
            }
        }
    }

    private void HandleRightClickAttack()
    {
        if (Input.GetMouseButtonDown(1)) // Botão direito
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100f);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    Transform enemy = hit.transform;

                    if (targetSelector != null)
                        targetSelector.SetTarget(enemy);

                    autoAttack.StartAutoAttack(enemy.gameObject); // mesmo se estiver longe

                    break; // para no primeiro inimigo encontrado
                }
            }
        }
    }

    private void HandleTargetChanged(GameObject newTarget)
    {
        autoAttack.StopAutoAttack(); // Para de atacar o alvo antigo
        autoAttack.SetTarget(newTarget); // Define o novo alvo, mas sem iniciar ataque ainda
    }

}
