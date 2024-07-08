using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI; // UI da loja
    public Button buyWeaponButton;
    public Button buyPotionButton;
    public Button exitButton; // Botão de sair
    public int weaponCost = 10;
    public int potionCost = 5;

    private bool weaponBought = false;
    private bool potionBought = false;
    private ThirdPersonCharacter playerCharacter;
    private PlayerInventory playerInventory;

    private void Start()
    {
        buyWeaponButton.onClick.AddListener(BuyWeapon);
        buyPotionButton.onClick.AddListener(BuyPotion);
        exitButton.onClick.AddListener(CloseShop); // Configura o botão de sair

        playerCharacter = FindObjectOfType<ThirdPersonCharacter>();
        playerInventory = FindObjectOfType<PlayerInventory>();

        UpdateButtonStates();
    }

    private void BuyWeapon()
    {
        if (!weaponBought && BananaManager.Instance.SpendBananas(weaponCost))
        {
            weaponBought = true;
            // Adiciona a lógica para dar a arma ao jogador
            playerInventory.AddItemToInventory(itemType.LancaCocos);
            UpdateButtonStates();
        }
    }

    private void BuyPotion()
    {
        if (!potionBought && BananaManager.Instance.SpendBananas(potionCost))
        {
            potionBought = true;
            // Adiciona a lógica para dar a poção ao jogador
            playerInventory.AddItemToInventory(itemType.Health);
            UpdateButtonStates();
        }
    }

    private void UpdateButtonStates()
    {
        buyWeaponButton.interactable = !weaponBought;
        buyPotionButton.interactable = !potionBought;
    }

    public void OpenShop()
    {
        if (playerCharacter != null)
        {
            ThirdPersonCharacter.IsShopOpen = true;
            playerCharacter.UnlockCursor();
        }
        shopUI.SetActive(true);
        Time.timeScale = 0f; // Pausa o jogo
    }

    public void CloseShop()
    {
        if (playerCharacter != null)
        {
            ThirdPersonCharacter.IsShopOpen = false;
            playerCharacter.LockCursor();
        }
        shopUI.SetActive(false);
        Time.timeScale = 1f; // Retoma o jogo
    }
}
