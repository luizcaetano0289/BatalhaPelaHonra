using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI spellName;

    private AbilityBase ability;

    public void Setup(AbilityBase ability)
    {
        this.ability = ability;
        icon.sprite = ability.icon;
        spellName.text = ability.abilityName;
    }
}
