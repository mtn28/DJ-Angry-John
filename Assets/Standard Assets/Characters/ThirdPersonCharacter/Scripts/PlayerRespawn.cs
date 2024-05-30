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
            Debug.Log("Jogador alcançou um checkpoint! Respawn point: " + transform.position);
            if (checkpointManager != null)
            {
                // Ajuste para definir o índice do checkpoint usando o array de checkpoints no CheckpointManager
                int checkpointIndex = System.Array.IndexOf(checkpointManager.checkpoints, transform);
                if (checkpointIndex >= 0)
                {
                    checkpointManager.SetCheckpointReached(checkpointIndex);
                }
                else
                {
                    Debug.LogError("Checkpoint não encontrado no array de checkpoints do CheckpointManager!");
                }
            }
            else
            {
                Debug.LogError("CheckpointManager não foi inicializado!");
            }
        }
    }
}
