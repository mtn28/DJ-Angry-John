using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    // Barra de preenchimento da vida no HUD
    public Image fillBar;
    // Valor atual da vida
    public float health;

    private void Awake()
    {
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
        // Verifica se a vida chegou a zero, o que quer dizer que morreu então chama o GsmeOver
        if (health <= 0)
        {
            // método GameOver do UIManager para exibir o fim de jogo
            //uiManager.GameOver();
        }
    }
}
