using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    // Nome da cena do jogo
    public string gameSceneName;

    // Referência ao VideoPlayer
    private VideoPlayer videoPlayer;

    private void Start()
    {
        // Obtém a referência ao VideoPlayer
        videoPlayer = GetComponent<VideoPlayer>();

        // Inscreve-se no evento que é disparado quando o vídeo termina
        videoPlayer.loopPointReached += OnVideoFinished;

        // Inicia a reprodução do vídeo
        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Carrega a cena do jogo quando o vídeo termina
        SceneManager.LoadScene(gameSceneName);
    }
}
