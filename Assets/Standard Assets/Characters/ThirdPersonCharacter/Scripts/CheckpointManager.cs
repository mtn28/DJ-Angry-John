using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Transform[] checkpoints;
    private int currentCheckpointIndex = -1;

    private void Start()
    {
        // Carrega o último checkpoint alcançado do PlayerPrefs
        currentCheckpointIndex = PlayerPrefs.GetInt("LastCheckpoint", -1);
    }

    public void SetCheckpointReached(int checkpointIndex)
    {
        currentCheckpointIndex = checkpointIndex;
        PlayerPrefs.SetInt("LastCheckpoint", currentCheckpointIndex);
    }

    public Vector3 GetRespawnPosition()
    {
        if (currentCheckpointIndex != -1 && currentCheckpointIndex < checkpoints.Length)
        {
            return checkpoints[currentCheckpointIndex].position;
        }
        else
        {
            // Se nenhum checkpoint foi alcançado, retorne a posição inicial do jogador ou outra posição padrão
            return checkpoints[0].position; // Supondo que o primeiro checkpoint seja a posição inicial
        }
    }

    public Quaternion GetRespawnRotation()
    {
        if (currentCheckpointIndex != -1 && currentCheckpointIndex < checkpoints.Length)
        {
            return checkpoints[currentCheckpointIndex].rotation;
        }
        else
        {
            // Se nenhum checkpoint foi alcançado, retorne a rotação inicial do jogador ou outra rotação padrão
            return checkpoints[0].rotation; // Supondo que a rotação do primeiro checkpoint seja a rotação inicial
        }
    }
}
