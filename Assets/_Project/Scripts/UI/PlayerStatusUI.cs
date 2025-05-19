using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour
{
    [SerializeField] private Image healthFill;
    [SerializeField] private Image manaFill;

    private float maxHealth = 100f;
    private float currentHealth = 100f;

    private float maxMana = 100f;
    private float currentMana = 100f;

    private void Update()
    {
        // Apenas para testes temporários: reduzir vida/mana com teclas
        if (Input.GetKeyDown(KeyCode.H)) currentHealth -= 10;
        if (Input.GetKeyDown(KeyCode.M)) currentMana -= 10;

        healthFill.fillAmount = currentHealth / maxHealth;
        manaFill.fillAmount = currentMana / maxMana;
    }
}
