using UnityEngine;

public class SpellbookToggleInput : MonoBehaviour
{
    public SpellbookUI spellbook;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            spellbook.ToggleVisibility();
    }
}
