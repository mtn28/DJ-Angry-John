using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.ThirdPerson;
using System.Collections.Generic;

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
    // Referência ao CheckpointManager
    [SerializeField]
    private CheckpointManager checkpointManager;

    // Referência ao ThirdPersonCharacter
    private ThirdPersonCharacter playerCharacter;

    // Para indicar se o jogo terminou
    private bool isOptions = false;

    // Lista de AudioSources que estavam tocando quando o jogo foi pausado
    private List<AudioSource> audioSourcesPlaying = new List<AudioSource>();

    private void Awake()
    {
        optionsScreen.SetActive(false);
        pauseScreen.SetActive(false);
        HUDScreen.SetActive(true);

        playerCharacter = FindObjectOfType<ThirdPersonCharacter>();
    }

    private void Update()
    {
        // Verifica se a tecla Esc foi pressionada
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isOptions)
            {
                // Se estiver no menu de opções, volta para o menu de pausa
                BackToPauseMenu();
            }
            else
            {
                // Se estiver no jogo, ativa ou desativa a tela de pausa
                PauseGame(!pauseScreen.activeInHierarchy);
            }
        }

        // Esconde o HUD se o jogo estiver pausado ou no menu de opções
        if (pauseScreen.activeInHierarchy || isOptions)
        {
            HUDScreen.SetActive(false);
        }
        else
        {
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
        playerCharacter.UnlockCursor(); // Garante que o cursor esteja desbloqueado
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
        isOptions = false;
        optionsScreen.SetActive(false);

        if (status)
        {
            playerCharacter.UnlockCursor();
            ThirdPersonCharacter.IsPaused = true;
            Time.timeScale = 0; // Pausa o jogo
            PauseAllAudio();
        }
        else
        {
            playerCharacter.LockCursor();
            ThirdPersonCharacter.IsPaused = false;
            Time.timeScale = 1; // Retoma o jogo
            ResumeAllAudio();
        }
    }

    // Método para pausar todos os sons
    private void PauseAllAudio()
    {
        audioSourcesPlaying.Clear();
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in allAudioSources)
        {
            if (audioSource.isPlaying)
            {
                audioSourcesPlaying.Add(audioSource);
                audioSource.Pause();
            }
        }
    }

    // Método para retomar todos os sons
    private void ResumeAllAudio()
    {
        foreach (AudioSource audioSource in audioSourcesPlaying)
        {
            audioSource.UnPause();
        }
        audioSourcesPlaying.Clear();
    }

    // Método para voltar ao menu de pausa a partir do menu de opções
    public void BackToPauseMenu()
    {
        isOptions = false;
        optionsScreen.SetActive(false);
        pauseScreen.SetActive(true);
        playerCharacter.UnlockCursor(); // Garante que o cursor esteja desbloqueado
    }

    // Método para ir ao menu de opções a partir do menu de pausa
    public void GoToOptionsMenu()
    {
        isOptions = true;
        optionsScreen.SetActive(true);
        pauseScreen.SetActive(false);
        playerCharacter.UnlockCursor(); // Garante que o cursor esteja desbloqueado
    }

    // Método para mover o jogador para um checkpoint específico
    public void MoveToCheckpoint()
    {
        if (checkpointManager != null)
        {
            Vector3 respawnPosition = checkpointManager.GetRespawnPosition();
            Quaternion respawnRotation = checkpointManager.GetRespawnRotation();
            playerCharacter.MoveToPosition(respawnPosition, respawnRotation);
            PauseGame(false);
        }
        else
        {
            Debug.LogWarning("CheckpointManager não foi atribuído no inspetor.");
        }
    }

    // Método para mover o jogador para a posição do boss
    public void MoveToBossCheckpoint()
    {
        Vector3 bossCheckpointPosition = new Vector3(-98f, -28.9673f, 94);
        Debug.LogError(bossCheckpointPosition);
        Quaternion bossCheckpointRotation = Quaternion.identity; // Defina a rotação desejada se necessário
        playerCharacter.MoveToPosition(bossCheckpointPosition, bossCheckpointRotation);
        PauseGame(false);
    }

    public bool IsPauseMenuActive()
    {
        return pauseScreen.activeInHierarchy;
    }

    public bool IsOptionsMenuActive()
    {
        return isOptions;
    }
}
