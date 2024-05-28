using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public Transform[] checkpoints;
    private int currentCheckpointIndex = -1;

    private void Start()
    {
        // Defina o último checkpoint alcançado como o checkpoint inicial
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
            // Se nenhum checkpoint foi alcançado, retorne a posição inicial do jogador
            return transform.position;
        }
    }
}
