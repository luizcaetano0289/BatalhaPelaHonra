using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform cameraPivot;

    private Animator animator;
    private CharacterController controller;

    public bool IsMoving { get; private set; }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        IsMoving = input.magnitude > 0.1f;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(h, 0, v).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // Transforma a direção do input com base na rotação da câmera
            Vector3 cameraForward = cameraPivot.forward;
            Vector3 cameraRight = cameraPivot.right;
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDir = (cameraForward * v + cameraRight * h).normalized;

            // Move e rotaciona o personagem para a direção do movimento
            controller.Move(moveDir * speed * Time.deltaTime);
            transform.forward = moveDir;

            animator?.SetFloat("Speed", moveDir.magnitude * speed);
        }
        else
        {
            animator?.SetFloat("Speed", 0f);
        }
    }
}
