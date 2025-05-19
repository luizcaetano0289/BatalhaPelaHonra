using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.PackageManager;

public class StatusPanelUI : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject panel;
    [SerializeField] private Button toggleButton;

    [Header("Campos de texto")]
    [SerializeField] private TextMeshProUGUI txtForca;
    [SerializeField] private TextMeshProUGUI txtAgilidade;
    [SerializeField] private TextMeshProUGUI txtInteligencia;
    [SerializeField] private TextMeshProUGUI txtEspirito;
    [SerializeField] private TextMeshProUGUI txtVigor;
    [SerializeField] private TextMeshProUGUI txtArmadura;
    [SerializeField] private TextMeshProUGUI txtHP;
    [SerializeField] private TextMeshProUGUI txtMana;
    [SerializeField] private TextMeshProUGUI txtAP;
    [SerializeField] private TextMeshProUGUI txtSP;
    [SerializeField] private TextMeshProUGUI txtCritico;
    [SerializeField] private TextMeshProUGUI txtEsquiva;
    [SerializeField] private TextMeshProUGUI txtParry;
    [SerializeField] private TextMeshProUGUI txtBlock;

    private void Start()
    {
        toggleButton.onClick.AddListener(TogglePainel);
        panel.SetActive(false);
    }

    private void UpdateAtributo(TextMeshProUGUI campo, int baseVal, int buffVal, int debuffVal, string nome)
    {
        int bonusVal = buffVal + debuffVal;
        int totalVal = baseVal + bonusVal;

        string bonusColor = bonusVal >= 0 ? "#00FF00" : "#FF0000";

        // Total fica vermelho se houver qualquer debuff
        string totalColor = "#FFFFFF";
        if (debuffVal < 0)
            totalColor = "#FF0000";
        else if (buffVal > 0)
            totalColor = "#00FF00";

        campo.text = $"{nome}: {baseVal} ({baseVal} + <color={bonusColor}>{bonusVal}</color>) = <color={totalColor}>{totalVal}</color>";
    }

    private void Update()
    {
        if (!panel.activeSelf) return;

        UpdateAtributo(txtForca, playerStats.baseStrength, playerStats.buffStrength, playerStats.debuffStrength, "Força");
        UpdateAtributo(txtAgilidade, playerStats.baseAgility, playerStats.buffAgility, playerStats.debuffAgility, "Agilidade");
        UpdateAtributo(txtInteligencia, playerStats.baseIntellect, playerStats.buffIntellect, playerStats.debuffIntellect, "Inteligência");
        UpdateAtributo(txtEspirito, playerStats.baseSpirit, playerStats.buffSpirit, playerStats.debuffSpirit, "Espírito");
        UpdateAtributo(txtVigor, playerStats.baseStamina, playerStats.buffStamina, playerStats.debuffStamina, "Vigor");
        UpdateAtributo(txtArmadura, playerStats.baseArmor, playerStats.buffArmor, playerStats.debuffArmor, "Armadura");

        txtHP.text = $"HP Máx: {playerStats.maxHealth}";
        txtMana.text = $"Mana Máx: {playerStats.maxMana}";
        txtAP.text = $"Poder de Ataque: {playerStats.attackPower:F1}";
        txtSP.text = $"Poder Mágico: {playerStats.spellPower:F1}";
        txtCritico.text = $"Crítico: {playerStats.critChance:F1}%";
        txtEsquiva.text = $"Esquiva: {playerStats.dodgeChance:F1}%";
        txtParry.text = $"Aparo: {playerStats.parryRating:F1}";
        txtBlock.text = $"Bloqueio: {playerStats.blockValue:F1}";
    }

    private void TogglePainel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
