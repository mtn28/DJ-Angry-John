using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipTrigger : MonoBehaviour
{
    public GameObject tipImage; // Arraste e solte a imagem da dica no Inspector
    public float displayDuration = 10f; // Duração da exibição em segundos
    private bool hasBeenTriggered = false; // Variável para controlar se a dica já foi exibida

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            hasBeenTriggered = true; // Marca que a dica já foi exibida
            ShowTip();
            Invoke("HideTip", displayDuration); // Invoca a desativação após a duração definida
        }
    }

    void ShowTip()
    {
        tipImage.SetActive(true);
    }

    void HideTip()
    {
        tipImage.SetActive(false);
        // Opcional: Desativar o objeto gatilho para garantir que não será ativado novamente
        gameObject.SetActive(false);
    }
}
