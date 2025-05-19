using UnityEngine;
using UnityEngine.UI;

public class ActionBarController : MonoBehaviour
{
    public Button[] slots;

    private void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Input.GetKeyDown((i + 1) % 10 == 0 ? KeyCode.Alpha0 : KeyCode.Alpha1 + i))
            {
                Debug.Log($"Slot {(((i + 1) % 10 == 0) ? 0 : (i + 1))} ativado!");
                HighlightSlot(i);
            }
        }
    }

    private void HighlightSlot(int index)
    {
        foreach (Button btn in slots)
            btn.image.color = Color.white;

        slots[index].image.color = Color.yellow;
    }
}
