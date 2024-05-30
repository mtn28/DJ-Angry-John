using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nome do nível a ser carregado ao iniciar um novo jogo
    public string nivelACarregar;

    // Referências para as telas
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private GameObject optionsScreen;

    private void Awake()
    {
        // Inicia com a tela do menu principal ativa e a de opções inativa
        mainMenuScreen.SetActive(true);
        optionsScreen.SetActive(false);

        // Pausa o tempo do jogo quando está no menu
        Time.timeScale = 0;
    }

    // Método para começar um novo jogo
    public void NewGame()
    {
        // Define a pontuação do jogador como 0 ao iniciar um novo jogo
        PlayerPrefs.SetInt("score", 0);

        // Carrega o nível especifico
        SceneManager.LoadScene(nivelACarregar);

        // Despausa o tempo do jogo ao iniciar um novo jogo
        Time.timeScale = 1;
    }

    // Método para abrir a tela de opções
    public void OpenOptions()
    {
        mainMenuScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    // Método para retornar ao menu principal a partir das opções
    public void BackToMenu()
    {
        mainMenuScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }

    // Método para carregar a cena das regras (opções de jogo)
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
