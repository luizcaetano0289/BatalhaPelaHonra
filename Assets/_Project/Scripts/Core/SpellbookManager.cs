using System.Collections.Generic;
using UnityEngine;

public class SpellbookManager : MonoBehaviour
{
    public static SpellbookManager Instance;

    public List<AbilityBase> geralMagias;
    public List<AbilityBase> arcanas;
    public List<AbilityBase> deGelo;
    public List<AbilityBase> deFogo;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public List<AbilityBase> GetAbilitiesByTab(SpellbookTab tab)
    {
        return tab switch
        {
            SpellbookTab.Arcano => arcanas,
            SpellbookTab.Gelo => deGelo,
            SpellbookTab.Fogo => deFogo,
            _ => geralMagias,
        };
    }
}
