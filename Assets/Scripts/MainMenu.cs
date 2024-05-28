using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nome do nível a ser carregado ao iniciar um novo jogo
    public string nivelACarregar;

    // Método para começar um novo jogo
    public void NewGame()
    {
        // Define a pontuação do jogador como 0 ao iniciar um novo jogo
        PlayerPrefs.SetInt("score", 0);

        // Carrega o nível especifico
        SceneManager.LoadScene(nivelACarregar);
    }

    // Método para carregar a cena das regras
    public void Options()
    {
        SceneManager.LoadScene("Options");
    }


    // Método para retornar ao menu principal
    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Método para sair do jogo
    public void QuitGame()
    {
        // Fecha o aplicativo
        Application.Quit();
    }

}
