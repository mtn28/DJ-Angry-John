using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    public AudioClip bulletSound;
    public float bulletSoundVolume = 1.0f; // Volume da bala ajustável
    private AudioSource audioSource;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("AngryJohn").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = gameObject.AddComponent<AudioSource>(); // Certifique-se de adicionar um novo AudioSource
    }

    private void Update()
    {
        // Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack code here
            Vector3 spawnPosition = transform.position + transform.forward * 1.5f + transform.up * 1.0f; // Adjust the offset as needed
            GameObject newProjectile = Instantiate(projectile, spawnPosition, Quaternion.identity);
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

            // Disable collider momentarily to avoid immediate collision with the enemy
            Collider projectileCollider = newProjectile.GetComponent<Collider>();
            if (projectileCollider != null)
            {
                projectileCollider.enabled = false;
                StartCoroutine(ReenableColliderAfterDelay(newProjectile, 0.1f));
            }

            // Adjust the direction to shoot slightly downward
            Vector3 forceDirection = transform.forward + transform.up * -0.1f; // Adjust the -0.1f to control how much downward force is applied
            rb.AddForce(forceDirection.normalized * 32f, ForceMode.Impulse);

            Destroy(newProjectile, 5f);
            // End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);

            // Play the bullet sound
            if (bulletSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(bulletSound, bulletSoundVolume);
            }
        }
    }

    private IEnumerator ReenableColliderAfterDelay(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Collider projectileCollider = projectile.GetComponent<Collider>();
        if (projectileCollider != null)
        {
            projectileCollider.enabled = true;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
