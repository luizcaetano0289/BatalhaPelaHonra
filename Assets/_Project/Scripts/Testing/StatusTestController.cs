using UnityEngine;
using UnityEngine.UI;

public class StatusTestController : MonoBehaviour
{
    [Header("Referência aos botões")]
    [SerializeField] private Button btnBuff;
    [SerializeField] private Button btnDebuff;

    [Header("Jogador")]
    [SerializeField] private PlayerStats playerStats;

    private void Start()
    {
        btnBuff.onClick.AddListener(AplicarBuff);
        btnDebuff.onClick.AddListener(AplicarDebuff);
    }

    void AplicarBuff()
    {
        playerStats.AplicarBuffTemporarioSTR(5, 10f); // 10 segundos
        playerStats.RecalculateDerivedStats();
    }

    void AplicarDebuff()
    {
        playerStats.AplicarDebuffTemporarioSTR(-2, 5f); // 5 segundos
        playerStats.RecalculateDerivedStats();
    }
}
