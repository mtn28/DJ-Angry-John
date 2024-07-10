using UnityEngine;

public class Banana : MonoBehaviour
{
    public AudioClip collectSound; // Som a ser reproduzido ao coletar a banana
    public GameObject audioSourcePrefab; // Prefab com um AudioSource

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o objeto colidido tem a tag "Player"
        if (other.CompareTag("Player"))
        {
            // Instancia um objeto temporário para reproduzir o som
            if (collectSound != null && audioSourcePrefab != null)
            {
                GameObject audioObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
                AudioSource audioSource = audioObject.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.clip = collectSound;
                    audioSource.Play();
                    // Destroi o objeto de som após a duração do clip
                    Destroy(audioObject, collectSound.length);
                }
            }

            // Chama o método para coletar a banana
            BananaManager.Instance.CollectBanana();

            // Destroi a banana imediatamente
            Destroy(gameObject);
        }
    }
}
