using System.Collections;
using UnityEngine;

namespace ArionDigital
{
    public class CrashCrate : MonoBehaviour
    {
        [Header("Whole Crate")]
        public MeshRenderer wholeCrate;
        public BoxCollider boxCollider;
        
        [Header("Fractured Crate")]
        public GameObject fracturedCrate;
        
        [Header("Audio")]
        public AudioSource crashAudioClip;

        [Header("Bananas")]
        public GameObject bananaPrefab; // Prefab da banana
        public int bananaCount = 5; // Quantidade de bananas a serem dropadas
        public float spawnRadius = 1.0f; // Raio ao redor da caixa para spawnar as bananas

        [Header("Fade Settings")]
        public Material transparentMaterial; // Material transparente

        // Lista para armazenar os Rigidbody das bananas
        private Rigidbody[] bananaRigidbodies;

        private void Start()
        {
            crashAudioClip.spatialBlend = 5.0f; 
            crashAudioClip.minDistance = 5.0f; 
            crashAudioClip.maxDistance = 15.0f;

            // Encontra todas as bananas dentro da caixa e desativa os Rigidbody
            bananaRigidbodies = GetComponentsInChildren<Rigidbody>();
            foreach (var rb in bananaRigidbodies)
            {
                if (rb.gameObject.CompareTag("Banana"))
                {
                    rb.isKinematic = true;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Verifica se o objeto colidido tem a tag "Banana"
            if (other.CompareTag("Banana"))
            {
                return;
            }

            // Ativa os Rigidbody das bananas
            foreach (var rb in bananaRigidbodies)
            {
                if (rb.gameObject.CompareTag("Banana"))
                {
                    rb.isKinematic = false;
                }
            }

            // Desabilita a caixa inteira e habilita a caixa fraturada
            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);
            crashAudioClip.Play();

            // Instancia as bananas
            DropBananas();

            // Ajusta os materiais da caixa fraturada para o material transparente
            SetMaterialsToTransparent(fracturedCrate);

            // Inicia a corutina para desvanecer a caixa fraturada
            StartCoroutine(FadeOutAndDestroy(fracturedCrate, 3f)); // Duração do fade-out de 3 segundos
        }

        [ContextMenu("Test")]
        public void Test()
        {
            // Ativa os Rigidbody das bananas
            foreach (var rb in bananaRigidbodies)
            {
                if (rb.gameObject.CompareTag("Banana"))
                {
                    rb.isKinematic = false;
                }
            }

            wholeCrate.enabled = false;
            boxCollider.enabled = false;
            fracturedCrate.SetActive(true);

            // Instancia as bananas
            DropBananas();

            // Ajusta os materiais da caixa fraturada para o material transparente
            SetMaterialsToTransparent(fracturedCrate);

            // Inicia a corutina para desvanecer a caixa fraturada
            StartCoroutine(FadeOutAndDestroy(fracturedCrate, 3f)); // Duração do fade-out de 3 segundos
        }

        private void DropBananas()
        {
            for (int i = 0; i < bananaCount; i++)
            {
                // Calcula uma posição aleatória ao redor da caixa
                Vector3 spawnPosition = transform.position + new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, Random.Range(-spawnRadius, spawnRadius));
                spawnPosition.y = transform.position.y + boxCollider.size.y / 2; // Define a altura para pairar na altura da caixa

                // Instancia a banana no local calculado
                GameObject banana = Instantiate(bananaPrefab, spawnPosition, Quaternion.identity);

                // Anexa o script Banana.cs à banana
                if (banana.GetComponent<Banana>() == null)
                {
                    banana.AddComponent<Banana>();
                }

                // Opcional: Desativa o Rigidbody da banana para fazê-la pairar
                Rigidbody rb = banana.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }
            }
        }

        private void SetMaterialsToTransparent(GameObject obj)
        {
            MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
            foreach (var renderer in meshRenderers)
            {
                renderer.material = transparentMaterial;
            }
        }

        private IEnumerator FadeOutAndDestroy(GameObject obj, float duration)
        {
            MeshRenderer[] meshRenderers = obj.GetComponentsInChildren<MeshRenderer>();
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

                foreach (var renderer in meshRenderers)
                {
                    Color color = renderer.material.color;
                    color.a = alpha;
                    renderer.material.color = color;
                }

                yield return null;
            }

            foreach (var renderer in meshRenderers)
            {
                Color color = renderer.material.color;
                color.a = 0;
                renderer.material.color = color;
            }

            Destroy(obj);
        }
    }
}
