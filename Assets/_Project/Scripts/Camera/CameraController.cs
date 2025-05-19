using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraPivot; // gira Y
    [SerializeField] private Transform cameraArm;   // gira X
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float pitchMin = -30f;
    [SerializeField] private float pitchMax = 60f;
    [SerializeField] private Transform playerTransform;

    private bool isRotating = false;
    private float yaw = 0f;
    private float pitch = 10f;

    private void Start()
    {
        yaw = cameraPivot.localEulerAngles.y;
        pitch = cameraArm.localEulerAngles.x;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) isRotating = true;
        if (Input.GetMouseButtonUp(1)) isRotating = false;

        if (isRotating)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

            cameraPivot.localRotation = Quaternion.Euler(0f, yaw, 0f);
            cameraArm.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            if (playerTransform != null)
            {
                Vector3 forward = cameraPivot.forward;
                forward.y = 0;
                playerTransform.forward = forward;
            }
        }
    }
}
