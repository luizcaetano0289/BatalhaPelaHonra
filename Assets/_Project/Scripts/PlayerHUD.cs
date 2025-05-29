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

    [Header("Cast Info")]
    [SerializeField] private TextMeshProUGUI txtCastName;

    private void Start()
    {
        if (castController != null)
        {
            castController.OnCastStart += ShowCastBar;
            castController.OnCastEnd += HideCastBar;
            castController.OnCastInterrupt += HideCastBar;
        }

        if (sliderCast != null)
            sliderCast.gameObject.SetActive(false);

        if (txtCastName != null)
            txtCastName.gameObject.SetActive(false); // 👈 Aqui você adiciona
    }



    private void Update()
    {
        // Vida e mana
        sliderHealth.maxValue = playerStats.maxHealth;
        sliderHealth.value = playerStats.currentHealth;

        sliderMana.maxValue = playerStats.maxMana;
        sliderMana.value = playerStats.currentMana;

        if (txtHealth != null)
            txtHealth.text = $"{sliderHealth.value} / {sliderHealth.maxValue}";
        if (txtMana != null)
            txtMana.text = $"{sliderMana.value} / {sliderMana.maxValue}";

        // Cast progress
        if (castController != null && castController.IsCasting && sliderCast != null)
        {
            sliderCast.value = castController.GetCastProgress();
        }
    }

    private void ShowCastBar()
    {
        if (sliderCast != null)
        {
            sliderCast.value = 0f;
            sliderCast.gameObject.SetActive(true);
        }

        if (txtCastName != null && castController != null)
        {
            txtCastName.gameObject.SetActive(true);
            txtCastName.text = $"Conjurando: {castController.SpellName}...";
        }
    }

    private void HideCastBar()
    {
        if (sliderCast != null)
            sliderCast.gameObject.SetActive(false);

        if (txtCastName != null)
        {
            txtCastName.text = "";
            txtCastName.gameObject.SetActive(false);
        }
    }

}
