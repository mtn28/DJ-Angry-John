using UnityEngine;
using TMPro;

public class BananaManager : MonoBehaviour
{
    public static BananaManager Instance;
    public TextMeshProUGUI bananaCounterText;
    private int bananaCount = 0;

    private void Awake()
    {
        bananaCounterText.text = "0";
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CollectBanana()
    {
        bananaCount++;
        UpdateBananaCount();
    }

    public bool SpendBananas(int amount)
    {
        if (bananaCount >= amount)
        {
            bananaCount -= amount;
            UpdateBananaCount();
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateBananaCount()
    {
        if (bananaCounterText != null)
        {
            bananaCounterText.text = bananaCount.ToString();
        }
    }

    public int GetBananaCount()
    {
        return bananaCount;
    }
}
