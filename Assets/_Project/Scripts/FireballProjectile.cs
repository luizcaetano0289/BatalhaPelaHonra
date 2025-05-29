using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 15f;
    public GameObject impactVFX;

    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    void Start()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            GetComponent<Rigidbody>().velocity = direction * speed;
            transform.forward = direction;
        }
        else
        {
            Debug.LogWarning("Fireball sem target. Será destruída.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target == null || other.transform != target) return;

        if (impactVFX != null)
        {
            Instantiate(impactVFX, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
