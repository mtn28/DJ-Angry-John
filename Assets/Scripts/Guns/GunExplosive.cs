using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunExplosive : MonoBehaviour
{
    // Assignables
    public Rigidbody rb;
    public GameObject explosionPrefab;
    public LayerMask whatIsEnemies;

    // Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    // Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;
    public int playerDamage = 5; // Dano causado ao jogador

    // Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    // Audio
    public AudioSource explosionAudioSource;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        // When to explode:
        if (collisions > maxCollisions) Explode();

        // Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    private void Explode()
    {
        Debug.Log("Chegou ao Explode");

        // Instantiate explosion
        if (explosionPrefab != null)
        {
            GameObject explosionInstance = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // Add and play the audio source
            if (explosionAudioSource != null)
            {
                AudioSource audioSourceInstance = explosionInstance.AddComponent<AudioSource>();
                audioSourceInstance.clip = explosionAudioSource.clip;
                audioSourceInstance.volume = explosionAudioSource.volume;
                audioSourceInstance.pitch = explosionAudioSource.pitch;
                audioSourceInstance.Play();

                // Optionally, you can destroy the AudioSource after the sound has finished playing
                Destroy(explosionInstance, explosionAudioSource.clip.length);
            }
            else
            {
                Destroy(explosionInstance, 3f); // Destroys explosion after 3 seconds if there's no sound
            }
        }

        // Check for enemies and player
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            // Get component of enemy and call TakeDamage
            EnemyAttack enemy = objectsInRange[i].GetComponent<EnemyAttack>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }

            // Add explosion force (if enemy has a rigidbody)
            if (objectsInRange[i].GetComponent<Rigidbody>())
            {
                objectsInRange[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            }
        }

        // Check for player
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, explosionRange);
        for (int i = 0; i < playersInRange.Length; i++)
        {
            if (playersInRange[i].CompareTag("Player"))
            {
                HealthManager player = playersInRange[i].GetComponent<HealthManager>();
                if (player != null)
                {
                    player.LoseHealth(playerDamage);
                }

                // Add explosion force to player (if player has a rigidbody)
                if (playersInRange[i].GetComponent<Rigidbody>())
                {
                    playersInRange[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
                }
            }
        }

        // Destroy the bullet after explosion
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Don't count collisions with other bullets
        if (collision.collider.CompareTag("BulletExplosive")) return;

        // Count up collisions
        collisions++;

        // Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Enemy") && explodeOnTouch)
        {
            Explode();
        }
    }

    private void Setup()
    {
        // Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        // Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        // Set gravity
        rb.useGravity = useGravity;
    }

    /// Just to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
