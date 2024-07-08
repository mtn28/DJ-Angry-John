using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private bool hasEntered = false; // Rastreador para garantir que a loja só abra uma vez por entrada

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEntered)
        {
            ShopManager shopManager = FindObjectOfType<ShopManager>();
            if (shopManager != null)
            {
                shopManager.OpenShop();
                hasEntered = true; // Marca como já entrou
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasEntered = false; // Redefine o rastreador quando o jogador sair
        }
    }
}
