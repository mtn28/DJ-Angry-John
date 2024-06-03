using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipManager : MonoBehaviour
{
    public static TipManager Instance { get; private set; }

    private Queue<GameObject> tipsQueue = new Queue<GameObject>();
    private bool isDisplayingTip = false;
    private UIManager uiManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            uiManager = FindObjectOfType<UIManager>(); // Find the UIManager instance in the scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTip(GameObject tip, float duration)
    {
        tipsQueue.Enqueue(tip);
        if (!isDisplayingTip)
        {
            StartCoroutine(DisplayTips(duration));
        }
    }

    private IEnumerator DisplayTips(float duration)
    {
        while (tipsQueue.Count > 0)
        {
            if (uiManager != null && (uiManager.IsPauseMenuActive() || uiManager.IsOptionsMenuActive()))
            {
                yield return null; // Wait until the pause or options menu is not active
            }
            else
            {
                isDisplayingTip = true;
                GameObject currentTip = tipsQueue.Dequeue();
                currentTip.SetActive(true);
                yield return new WaitForSeconds(duration);
                currentTip.SetActive(false);
            }
        }
        isDisplayingTip = false;
    }
}
