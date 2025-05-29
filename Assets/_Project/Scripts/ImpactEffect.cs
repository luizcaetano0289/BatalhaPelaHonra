using UnityEngine;

public class ImpactEffect : MonoBehaviour
{
    public float duration = 2f;

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
