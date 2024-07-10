using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class BossAttack : MonoBehaviour
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
    public GameObject powerfulProjectile; // Novo projétil mais poderoso
    public float powerfulProjectileDamage = 30f; // Dano do projétil mais poderoso

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private float powerfulProjectileCooldown = 12f;
    private bool canShootPowerfulProjectile = true;

    public AudioSource bulletAudioSource; // Referência ao AudioSource para o som do projétil
    public float bulletSoundVolume = 1.0f; // Volume da bala ajustável

    public GameObject explosionEffect; // Referência ao prefab de explosão

    private bool hasExploded = false; // Variável para garantir que a explosão ocorra uma vez

    private void Awake()
    {
        player = GameObject.Find("AngryJohn").transform;
        agent = GetComponent<NavMeshAgent>();

        // Adicione um novo AudioSource se não houver nenhum atribuído
        if (bulletAudioSource == null)
        {
            bulletAudioSource = gameObject.AddComponent<AudioSource>();
            bulletAudioSource.volume = bulletSoundVolume;
            bulletAudioSource.playOnAwake = false;
        }
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
        // Certifique-se de que o inimigo está olhando para o jogador
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        if (!alreadyAttacked)
        {
            if (canShootPowerfulProjectile)
            {
                ShootPowerfulProjectile(directionToPlayer);
            }
            else
            {
                ShootRegularProjectile(directionToPlayer);

                // Play the bullet sound
                if (bulletAudioSource != null && bulletAudioSource.clip != null)
                {
                    bulletAudioSource.Play();
                }
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ShootRegularProjectile(Vector3 directionToPlayer)
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2.0f; // Ajuste para uma posição segura
        Vector3 elevatedDirection = (directionToPlayer + Vector3.up * 0.1f).normalized; // Adiciona uma elevação à direção
        GameObject newProjectile = Instantiate(projectile, spawnPosition, Quaternion.LookRotation(elevatedDirection));
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

        // Desativar o collider inicialmente
        Collider projectileCollider = newProjectile.GetComponent<Collider>();
        if (projectileCollider != null)
        {
            projectileCollider.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(projectileCollider, 0.1f));
        }

        // Ajuste a direção para atirar diretamente para o jogador
        rb.velocity = elevatedDirection * 32f;

        Destroy(newProjectile, 5f);
    }

    private void ShootPowerfulProjectile(Vector3 directionToPlayer)
    {
        Vector3 spawnPosition = transform.position + transform.forward * 2.0f; // Ajuste para uma posição segura
        Vector3 elevatedDirection = (directionToPlayer + Vector3.up * 0.1f).normalized; // Adiciona uma elevação à direção
        GameObject newProjectile = Instantiate(powerfulProjectile, spawnPosition, Quaternion.LookRotation(elevatedDirection));
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

        // Desativar o collider inicialmente
        Collider projectileCollider = newProjectile.GetComponent<Collider>();
        if (projectileCollider != null)
        {
            projectileCollider.enabled = false;
            StartCoroutine(EnableColliderAfterDelay(projectileCollider, 0.1f));
        }

        // Ajuste a direção para atirar diretamente para o jogador
        rb.velocity = elevatedDirection * 32f;

        Destroy(newProjectile, 5f);
        canShootPowerfulProjectile = false;
        Invoke(nameof(ResetPowerfulProjectileCooldown), powerfulProjectileCooldown);
    }

    private IEnumerator EnableColliderAfterDelay(Collider collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (collider != null)
        {
            collider.enabled = true;
        }
    }

    private void ResetPowerfulProjectileCooldown()
    {
        canShootPowerfulProjectile = true;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0 && !hasExploded) // Verifica se o Boss já foi destruído
        {
            hasExploded = true;
            Invoke(nameof(DestroyEnemy), 0.5f);
            SceneManager.LoadScene("CutScene 1");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "JohnBullet")
        {
            TakeDamage(50);
        }
        else if (collision.gameObject.tag == "BulletExplosive")
        {
            TakeDamage(100);
        }
    }

    private void DestroyEnemy()
    {
        if (explosionEffect != null)
        {
            GameObject explosionInstance = Instantiate(explosionEffect, transform.position, transform.rotation); // Instancia o efeito de explosão
            Destroy(explosionInstance, 3f); // Destrói o efeito de explosão após 3 segundos
        }
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
