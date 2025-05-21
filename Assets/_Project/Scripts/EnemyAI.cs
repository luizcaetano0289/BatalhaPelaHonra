using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Comportamento")]
    public float chaseDistance = 5f; // Distância configurável no Inspector

    private NavMeshAgent agent;
    private Animator animator;

    private bool isMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("IgnoreRaycastPlayer");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseDistance)
        {
            agent.SetDestination(player.position);
            isMoving = true;
        }
        else
        {
            agent.ResetPath();
            isMoving = false;
        }

        if (animator != null)
        {
            animator.SetBool("IsMoving", isMoving);
        }
    }

    public bool IsPlayerInChaseRange()
    {
        if (player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= chaseDistance;
    }
}
