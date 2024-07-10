using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BigRookGames.Weapons
{
    public class ProjectileController : MonoBehaviour
    {
        public float speed = 100;
        public LayerMask collisionLayerMask;

        public GameObject rocketExplosion;
        public MeshRenderer projectileMesh;

        private bool targetHit;

        public AudioSource inFlightAudioSource;
        public ParticleSystem disableOnHit;

        public float explosionRadius = 5f;
        public int damage = 40;

        public HealthManager healthManager;

        private void Start()
        {
            // Ajuste a posição inicial do projétil
            transform.position += new Vector3(0, 0.5f, 0);

            // Ensures the projectile collider is enabled after a short delay
            StartCoroutine(EnableColliderAfterDelay(0.1f));

            // Encontrar o HealthManager mais próximo ao inicializar
            healthManager = FindClosestHealthManager();
        }

        private IEnumerator EnableColliderAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            Collider projectileCollider = GetComponent<Collider>();
            if (projectileCollider != null)
            {
                projectileCollider.enabled = true;
            }
        }

        private void Update()
        {
            if (targetHit) return;

            // Move o projétil na direção em que foi instanciado
            transform.position += transform.forward * (speed * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!enabled) return;

            Explode();
            projectileMesh.enabled = false;
            targetHit = true;
            inFlightAudioSource.Stop();
            foreach (Collider col in GetComponents<Collider>())
            {
                col.enabled = false;
            }
            disableOnHit.Stop();

            Destroy(gameObject, 5f);
        }

        private void Explode()
        {
            GameObject newExplosion = Instantiate(rocketExplosion, transform.position, rocketExplosion.transform.rotation, null);
            ApplyAreaDamage(transform.position, explosionRadius, damage);
        }

        private void ApplyAreaDamage(Vector3 explosionPoint, float explosionRadius, int damage)
        {
            Collider[] colliders = Physics.OverlapSphere(explosionPoint, explosionRadius);
            HashSet<HealthManager> damagedHealthManagers = new HashSet<HealthManager>();

            foreach (Collider hit in colliders)
            {
                HealthManager targetHealth = hit.GetComponent<HealthManager>();
                if (targetHealth != null && !damagedHealthManagers.Contains(targetHealth))
                {
                    targetHealth.LoseHealth(damage);
                    damagedHealthManagers.Add(targetHealth);
                }

                Rigidbody rb = hit.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddExplosionForce(1000f, explosionPoint, explosionRadius);
                }
            }
        }

        private HealthManager FindClosestHealthManager()
        {
            HealthManager[] allHealthManagers = FindObjectsOfType<HealthManager>();
            HealthManager closest = null;
            float closestDistance = Mathf.Infinity;
            Vector3 currentPosition = transform.position;

            foreach (HealthManager healthManager in allHealthManagers)
            {
                float distance = Vector3.Distance(currentPosition, healthManager.transform.position);
                if (distance < closestDistance)
                {
                    closest = healthManager;
                    closestDistance = distance;
                }
            }

            return closest;
        }
    }
}
