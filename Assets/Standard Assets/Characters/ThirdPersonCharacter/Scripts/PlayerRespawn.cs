using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public CheckpointManager checkpointManager;

    private void Start()
    {
        // Exemplo de como encontrar o CheckpointManager na cena se ele não foi atribuído manualmente
        if (checkpointManager == null)
        {
            checkpointManager = FindObjectOfType<CheckpointManager>();
            if (checkpointManager == null)
            {
                Debug.LogError("CheckpointManager não encontrado na cena!");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Quando o jogador entra em um objeto com a tag "RespawnPoint", ele alcançou um checkpoint
            Debug.Log("Jogador alcançou um checkpoint! Respawn point: " + transform.position);
            if (checkpointManager != null)
            {
                checkpointManager.SetCheckpointReached(transform.GetSiblingIndex());
            }
            else
            {
                Debug.LogError("CheckpointManager não foi inicializado!");
            }
        }
    }
}
