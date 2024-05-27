using UnityEngine;

public class AmbienteSoundController : MonoBehaviour
{
    private AudioSource ambienteSound;
    public string audioFileName = "selvaambiente"; // Nome do arquivo de áudio

    void Start()
    {
        ambienteSound = gameObject.AddComponent<AudioSource>(); // Adiciona o AudioSource
        ambienteSound.loop = true; // Ativa o loop
        PlayAmbienteSound(); // Inicia a reprodução do som de ambiente
    }

    void PlayAmbienteSound()
    {
        // Verifica se o nome do arquivo está correto
        if (string.IsNullOrEmpty(audioFileName))
        {
            Debug.LogError("Nome do arquivo de áudio não definido.");
            return;
        }

        AudioClip clip = Resources.Load<AudioClip>(audioFileName); // Carrega o arquivo de áudio
        if (clip != null)
        {
            ambienteSound.clip = clip; // Atribui o áudio carregado ao AudioSource
            ambienteSound.Play(); // Reproduz o som de ambiente
        }
        else
        {
            Debug.LogError("Erro ao carregar o som de ambiente: " + audioFileName + " não encontrado.");
        }
    }
}
