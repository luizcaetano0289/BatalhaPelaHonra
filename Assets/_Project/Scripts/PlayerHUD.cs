using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] public PlayerStats playerStats;
    [SerializeField] public CastController castController;

    [Header("Sliders")]
    [SerializeField] private Slider sliderHealth;
    [SerializeField] private Slider sliderMana;
    [SerializeField] private Slider sliderCast;

    [Header("Textos (opcional)")]
    [SerializeField] private TextMeshProUGUI txtHealth;
    [SerializeField] private TextMeshProUGUI txtMana;

    private void Update()
    {
        // Vida
        sliderHealth.maxValue = playerStats.maxHealth;
        sliderHealth.value = playerStats.currentHealth;

        // Mana
        sliderMana.maxValue = playerStats.maxMana;
        sliderMana.value = playerStats.currentMana;

        // Texto Vida
        if (txtHealth != null)
            txtHealth.text = $"{sliderHealth.value} / {sliderHealth.maxValue}";

        // Texto Mana
        if (txtMana != null)
            txtMana.text = $"{sliderMana.value} / {sliderMana.maxValue}";

        // Cast
        if (castController != null && sliderCast != null)
        {
            if (castController.IsCasting)
            {
                sliderCast.gameObject.SetActive(true);
                sliderCast.maxValue = 1f;
                sliderCast.value = castController.GetCastProgress();
            }
            else
            {
                sliderCast.gameObject.SetActive(false);
            }
        }
    }
}
