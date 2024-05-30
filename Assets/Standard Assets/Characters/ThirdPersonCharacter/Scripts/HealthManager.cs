using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;
public class HealthManager : MonoBehaviour
{
    // Barra de preenchimento da vida no HUD
    public Image fillBar;
    // Valor atual da vida
    public float health;

    private ThirdPersonCharacter character;

    private void Awake()
    {
        character = FindObjectOfType<ThirdPersonCharacter>();
    }

    // Método chamado para reduzir a saúde do jogador
    public void LoseHealth(int value)
    {
        // Não faz nada se a saúde já estiver a zeros
        if (health <= 0)
            return;

        // Reduz a vida pelo valor dado pelo parâmetro
        health -= value;
        // Atualiza a quantidade preenchida da barra de vida no HUD
        fillBar.fillAmount = health / 100;

        // Verifica se a vida chegou a zero, o que quer dizer que morreu então chama o Die
        if (health <= 0)
        {
            character.Die();
        }
    }

    // Método para resetar a vida do jogador
    public void ResetHealth()
    {
        health = 100;
        fillBar.fillAmount = health / 100;
    }
}
