using UnityEngine;

public class CooldownManager : MonoBehaviour
{
    public static CooldownManager Instance { get; private set; }

    [Header("Global Cooldown")]
    [SerializeField] private float globalCooldownDuration = 1.5f;
    private float globalCooldownTimer = 0f;

    public bool IsGlobalCooldownActive => globalCooldownTimer > 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (globalCooldownTimer > 0f)
        {
            globalCooldownTimer -= Time.deltaTime;
        }
    }

    public void TriggerGlobalCooldown()
    {
        globalCooldownTimer = globalCooldownDuration;
    }
}
