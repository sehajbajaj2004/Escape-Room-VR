using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public NavMeshAgent agent;

    [Header("Settings")]
    public float sightRange = 7f;
    public float patrolRadius = 5f;

    private Transform player;
    private bool isDead = false;
    private Vector3 patrolTarget;
    private float baseSpeed;

    private enum State { Patrol, Chase, Dead }
    private State currentState = State.Patrol;

    private void Start()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponent<Animator>();

        baseSpeed = agent.speed;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) player = playerObj.transform;

        SetNewPatrolPoint();
    }

    private void Update()
    {
        if (isDead || !player) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (distToPlayer <= sightRange)
            currentState = State.Chase;
        else
            currentState = State.Patrol;

        if (currentState == State.Patrol) Patrol();
        else if (currentState == State.Chase) Chase();
    }

    private void Patrol()
    {
        agent.speed = baseSpeed;
        animator.SetBool("isWalking", true);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            SetNewPatrolPoint();
        }
    }

    private void Chase()
    {
        agent.speed = baseSpeed * 1.5f; // run a bit faster
        animator.SetBool("isWalking", true);
        agent.SetDestination(player.position);
    }

    private void SetNewPatrolPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * patrolRadius + transform.position;
        if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, patrolRadius, NavMesh.AllAreas))
        {
            patrolTarget = hit.position;
            agent.SetDestination(patrolTarget);
        }
    }

    public void TakeDamage()
    {
        if (isDead) return;
        isDead = true;

        agent.speed = 0f;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("Die");
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage();
            Destroy(other.gameObject); // remove bullet on hit
        }
    }
}
