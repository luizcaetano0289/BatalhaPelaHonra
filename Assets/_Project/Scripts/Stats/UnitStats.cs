using UnityEngine;

public class UnitStats : MonoBehaviour
{
    [Header("Vida")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Cast")]
    public bool isCasting = false;
    public float castProgress = 0f; // 0 a 1 (progresso da magia)

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public float GetHealthNormalized()
    {
        return (float)currentHealth / maxHealth;
    }

    public float GetCastProgress()
    {
        return isCasting ? castProgress : 0f;
    }

    // Métodos auxiliares para simular casting
    public void StartCast()
    {
        isCasting = true;
        castProgress = 0f;
    }

    public void UpdateCast(float amount)
    {
        if (!isCasting) return;

        castProgress += amount;
        if (castProgress >= 1f)
        {
            castProgress = 1f;
            isCasting = false;
        }
    }

    private void Update()
    {
       
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"{gameObject.name} perdeu {amount} de HP. Restam: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        Debug.Log($"{gameObject.name} morreu.");
        gameObject.SetActive(false); // ou qualquer lógica de morte
    }

}
