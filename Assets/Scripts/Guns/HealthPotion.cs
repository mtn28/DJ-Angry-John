using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    public int healthIncrease = 25; // Valor da saúde aumentada pela poção
    public AudioClip drinkSound; // Som da poção sendo usada
    private AudioSource audioSource;
    private HealthManager healthManager;
    private PlayerInventory playerInventory;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        healthManager = FindObjectOfType<HealthManager>();
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    void Update()
    {
        // Detecta se o jogador clicou no botão esquerdo do mouse
        if (Input.GetMouseButtonDown(0))
        {
            UsePotion();
        }
    }

    void UsePotion()
    {
        // Limita a saúde do jogador ao máximo de 100
        if (healthManager.health >= 100)
        {
            healthManager.health = 100;
        }
        else
        {
            // Aumenta a saúde do jogador
            healthManager.health += healthIncrease;
            // Atualiza a barra de vida no HUD
            healthManager.fillBar.fillAmount = healthManager.health / 100;

            // Toca o som da poção sendo usada
            audioSource.PlayOneShot(drinkSound);

            // Notifica o PlayerInventory para remover a poção do inventário
            playerInventory.UseHealthPotion();

        }
    }
}
