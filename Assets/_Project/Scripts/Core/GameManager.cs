using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TargetSelector TargetSelector { get; private set; }

    public CombatManager CombatManager { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        TargetSelector = GetComponent<TargetSelector>();

        CombatManager = GetComponent<CombatManager>();
    }
}
