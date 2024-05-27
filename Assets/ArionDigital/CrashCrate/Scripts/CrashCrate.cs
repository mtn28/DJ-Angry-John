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

        // Lista para armazenar os Rigidbody das bananas
        private Rigidbody[] bananaRigidbodies;

        private void Start()
        {
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
        }
    }
}
