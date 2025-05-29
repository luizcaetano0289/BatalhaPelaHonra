using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpellbookUI : MonoBehaviour
{
    [Header("Referências")]
    public Button btnArcano;
    public Button btnGelo;
    public Button btnFogo;

    public Transform spellGrid;
    public GameObject spellSlotPrefab;

    private SpellbookTab currentTab = SpellbookTab.Geral;

    private void Start()
    {
        btnArcano.onClick.AddListener(() => ToggleTab(SpellbookTab.Arcano));
        btnGelo.onClick.AddListener(() => ToggleTab(SpellbookTab.Gelo));
        btnFogo.onClick.AddListener(() => ToggleTab(SpellbookTab.Fogo));

        ShowTab(SpellbookTab.Geral); // sempre começa na aba geral
    }

    private void ToggleTab(SpellbookTab tab)
    {
        if (currentTab == tab)
            ShowTab(SpellbookTab.Geral); // clicou na mesma aba = volta pra geral
        else
            ShowTab(tab);
    }

    private void ShowTab(SpellbookTab tab)
    {
        currentTab = tab;

        foreach (Transform child in spellGrid)
            Destroy(child.gameObject);

        List<AbilityBase> magias = SpellbookManager.Instance.GetAbilitiesByTab(tab);

        foreach (var magia in magias)
        {
            GameObject slot = Instantiate(spellSlotPrefab, spellGrid);
            slot.GetComponent<SpellSlotUI>().Setup(magia);
        }
    }

    public void ToggleVisibility()
    {
        gameObject.SetActive(!gameObject.activeSelf);

        if (gameObject.activeSelf)
            ShowTab(SpellbookTab.Geral);
    }
}
