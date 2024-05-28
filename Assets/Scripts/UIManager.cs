using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Tela de Options
    [SerializeField]
    private GameObject optionsScreen;
    // Tela de pausa
    [SerializeField]
    private GameObject pauseScreen;
    // HUD
    [SerializeField]
    private GameObject HUDScreen;
    [SerializeField]
    private AudioSource backgroundMusicSource;
    // Para indicar se o jogo terminou
    private bool isOptions = false;

    private void Awake()
    {
        optionsScreen.SetActive(false);
        pauseScreen.SetActive(false);
        HUDScreen.SetActive(true);
    }

    void Start()
    {
    }

    private void Update()
    {
        // Verifica se a tecla Esc foi pressionada e se o jogo não terminou
        if (Input.GetKeyDown(KeyCode.Escape) && !isOptions)
        {
            // Se a tela de pausa estiver ativa, desativa-a, caso contrário, ativa-a
            PauseGame(!pauseScreen.activeInHierarchy);
        }

        // Esconde o HUD se o jogo estiver pausado
        if (pauseScreen.activeInHierarchy || isOptions)
        {
            // Pausa a música de fundo
            if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Pause();
            }
            // Esconde o HUD
            HUDScreen.SetActive(false);
        }
        else
        {
            // Despausa a música de fundo
            if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.UnPause();
            }
            // Mostra o HUD
            HUDScreen.SetActive(true);
        }
    }

    // Ativa a tela de Options
    public void Options()
    {
        isOptions = true;
        optionsScreen.SetActive(true);
        HUDScreen.SetActive(false); // Esconde o HUD
        pauseScreen.SetActive(false);
        Time.timeScale = 0; // Pausa o tempo do jogo
    }

    // Retorna para o menu principal
    public void MainMenu()
    {
        isOptions = false;
        SceneManager.LoadScene("Menu");
        PauseGame(false);
    }

    // Sai do jogo
    public void Quit()
    {
        Application.Quit();
    }

    // Método para pausar ou despausar o jogo
    public void PauseGame(bool status)
    {
        // Ativa ou desativa a tela de pausa
        pauseScreen.SetActive(status);
        // Quando a pausa está ativada, o timescale é definido como 0 e o tempo está parado
        // Quando está desativada, o timescale é definido como 1 e o tempo continua normalmente
        Time.timeScale = status ? 0 : 1;
    }
}
