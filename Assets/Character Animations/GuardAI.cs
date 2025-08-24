using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GuardAI : MonoBehaviour
{
    [Header("Navigation Settings")]
    public float patrolRadius = 10f;
    public float patrolTimer = 5f;
    
    [Header("Detection Settings")]
    public float detectionRadius = 8f;
    public float fieldOfView = 90f;
    
    [Header("Combat Settings")]
    public float shootingRate = 0.5f;
    public float bulletSpeed = 20f;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    // public AudioClip shootSound;
    public float bulletLife = 3f;
    public AudioSource audioSource;
    
    private NavMeshAgent agent;
    private Animator animator;
    private Transform player;
    private float currentPatrolTimer;
    private bool isAiming;
    private bool isDead;
    private Vector3 initialPosition;
    private float shootingTimer;
    private bool hasDrawnGun;
    private Coroutine combatCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
        currentPatrolTimer = patrolTimer;
        shootingTimer = shootingRate;
        
        // Start patrolling
        SetRandomDestination();
    }

    void Update()
    {
        if (isDead) return;
        
        // Check if player is in detection range
        bool playerDetected = CheckForPlayer();
        
        if (playerDetected && !isAiming)
        {
            // Start combat routine
            if (combatCoroutine != null) StopCoroutine(combatCoroutine);
            combatCoroutine = StartCoroutine(StartCombat());
        }
        else if (isAiming && !playerDetected)
        {
            // Player left detection range, stop combat
            if (combatCoroutine != null) StopCoroutine(combatCoroutine);
            combatCoroutine = StartCoroutine(StopCombat());
        }
        else if (isAiming)
        {
            // Continue aiming and shooting at player
            FacePlayer();
            
            // Handle shooting timer - trigger shoot animation periodically
            shootingTimer -= Time.deltaTime;
            if (shootingTimer <= 0)
            {
                shootingTimer = shootingRate;
                animator.SetTrigger("Shoot");
            }
        }
        else
        {
            // Continue with normal patrol behavior
            currentPatrolTimer += Time.deltaTime;
            
            // Set new destination when timer expires or reached destination
            if (currentPatrolTimer >= patrolTimer || (agent.remainingDistance < 0.5f && !agent.pathPending))
            {
                SetRandomDestination();
                currentPatrolTimer = 0;
            }
        }
        
        // Update animator
        UpdateAnimation();
    }
    
    // This method will be called by the animation event
    public void FireBullet()
    {
        // Play shoot sound
        if (audioSource != null)
        {
            audioSource.PlayOneShot();
        }

        // Spawn bullet
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Quaternion adjustedRotation = bulletSpawnPoint.rotation * Quaternion.Euler(0, 90, 0);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, adjustedRotation);

            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = bulletSpawnPoint.forward * bulletSpeed;
            }

            // Destroy bullet after some time
            Destroy(bullet, bulletLife);
        }
    }
    
    IEnumerator StartCombat()
    {
        isAiming = true;
        
        // Stop the agent
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        
        // Play draw gun animation if not already drawn
        if (!hasDrawnGun)
        {
            animator.SetTrigger("DrawGun");
            hasDrawnGun = true;
            
            // Wait for draw animation to complete
            yield return new WaitForSeconds(1.0f);
        }
        
        // Start shooting loop
        animator.SetBool("IsShooting", true);
    }
    
    IEnumerator StopCombat()
    {
        isAiming = false;
        
        // Stop shooting animation
        animator.SetBool("IsShooting", false);
        
        // Wait a moment before resuming movement
        yield return new WaitForSeconds(0.5f);
        
        // Re-enable the agent
        agent.isStopped = false;
        
        // Return to patrol
        SetRandomDestination();
    }
    
    void FacePlayer()
    {
        // Rotate towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Keep rotation only on Y axis
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
    
    bool CheckForPlayer()
    {
        // Check if player is within detection radius
        if (Vector3.Distance(transform.position, player.position) > detectionRadius)
            return false;
        
        // Check if player is in field of view
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);
        
        if (angleToPlayer < fieldOfView / 2)
        {
            // Check for obstacles between guard and player
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
            }
        }
        
        return false;
    }
    
    void SetRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += initialPosition;
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    
    void UpdateAnimation()
    {
        if (animator != null)
        {
            // Set walking animation based on agent velocity (only when not aiming)
            if (!isAiming)
            {
                bool isMoving = agent.velocity.magnitude > 0.1f && agent.remainingDistance > 0.5f;
                animator.SetBool("IsWalking", isMoving);
            }
            else
            {
                // Not walking when aiming
                animator.SetBool("IsWalking", false);
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;
        
        // Check if hit by a bullet
        if (other.CompareTag("Bullet"))
        {
            Die();
            
            // Optional: Destroy the bullet
            Destroy(other.gameObject);
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;
        
        // Check if hit by a bullet (in case bullet uses collision instead of trigger)
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Die();
            
            // Optional: Destroy the bullet
            Destroy(collision.gameObject);
        }
    }
    
    void Die()
    {
        isDead = true;
        
        // Stop all movements and combat
        agent.isStopped = true;
        isAiming = false;
        
        // Stop any coroutines
        if (combatCoroutine != null)
        {
            StopCoroutine(combatCoroutine);
        }
        
        // Play death animation
        animator.SetBool("IsShooting", false);
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Die");
        
        // Disable the script after death
        enabled = false;
        
        // Optional: Disable collider to prevent further interactions
        Collider collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
        
        // Optional: Disable NavMeshAgent
        if (agent != null) agent.enabled = false;
    }
    
    // Visualize detection radius and field of view in editor
    void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        // Draw patrol radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Application.isPlaying ? initialPosition : transform.position, patrolRadius);
        
        // Draw field of view
        Gizmos.color = Color.red;
        Vector3 leftBoundary = Quaternion.Euler(0, -fieldOfView/2, 0) * transform.forward * detectionRadius;
        Vector3 rightBoundary = Quaternion.Euler(0, fieldOfView/2, 0) * transform.forward * detectionRadius;
        
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
        
        // Draw bullet spawn point if assigned
        if (bulletSpawnPoint != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(bulletSpawnPoint.position, 0.1f);
            Gizmos.DrawLine(bulletSpawnPoint.position, bulletSpawnPoint.position + bulletSpawnPoint.forward * 0.5f);
        }
    }
}