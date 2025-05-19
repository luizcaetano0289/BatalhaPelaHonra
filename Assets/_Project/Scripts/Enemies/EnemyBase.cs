using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public string enemyName = "Inimigo";
    public int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{enemyName} recebeu {damage} de dano. Vida atual: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{enemyName} foi derrotado!");
        Destroy(gameObject);
    }
}
