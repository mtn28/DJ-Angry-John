using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    public GameObject tipImage; // Arraste e solte a imagem da dica no Inspector
    public float displayDuration = 5f; // Duração da exibição em segundos
    private bool hasBeenTriggered = false; // Variável para controlar se a dica já foi exibida

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true; // Marca que a dica já foi exibida
            TipManager.Instance.ShowTip(tipImage, displayDuration);
        }
    }
}
