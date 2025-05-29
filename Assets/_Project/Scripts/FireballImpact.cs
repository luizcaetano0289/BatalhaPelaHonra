using UnityEngine;

public class FireballImpact : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Apenas destrói se colidir com algo que não seja o próprio jogador
        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
