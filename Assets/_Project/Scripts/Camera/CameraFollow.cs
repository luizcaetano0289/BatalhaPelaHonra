using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null) return;

        // Movimento seco, sem delay
        transform.position = Vector3.Lerp(transform.position, target.position + offset, 50f * Time.deltaTime);
    }
}
