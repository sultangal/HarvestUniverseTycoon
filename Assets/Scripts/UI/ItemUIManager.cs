using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemUIManager : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform itemUITemplate;
    private bool startCountFlag = false;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            Reinitialize();
            startCountFlag = true;
        }
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsTimeIsUp())
        {
            startCountFlag = false;
        }

        if (GameManager.Instance.IsGameWaitingToStart())
        {
            ResetItems();
        }

    }

    private void ResetItems()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    private void Reinitialize()
    {
        ResetItems();

        for (int i = 0; i < Planets.Instance.GetCurrentPlanetSO().fieldItemAmountForNextLevel.Length; i++)
        {
            var item = Instantiate(itemUITemplate, container);
            var itemUI = item.GetComponent<ItemUI>();
            itemUI.countsGoal.text = "/" +
                Planets.Instance.GetCurrentPlanetSO().fieldItemAmountForNextLevel[i].ToString();
            itemUI.itemImage.sprite =
                Planets.Instance.GetCurrentPlanetSO().fieldItemSOs[i].itemSprite;
        }
    }

    private void UpdateVisuals()
    {
        var gm = GameManager.Instance;
        var pln = Planets.Instance;
        for (int i = 0; i < gm.GameSessionData_.CollectedFieldItems.Length; i++)
        {
            int countCollected =
                pln.GetCurrentPlanetAmountOfCollectedItems()[i] + gm.GameSessionData_.CollectedFieldItems[i];
            //int countCollected = gm.GameSessionData_.CollectedFieldItems[i];
            int countsGoal = pln.GetCurrentPlanetSO().fieldItemAmountForNextLevel[i];
            var itemUIComponent = container.GetChild(i).GetComponent<ItemUI>();
            if (countCollected > countsGoal)
            {
                itemUIComponent.countsCollected.color = new Color(0.0f, 1.0f, 0.0f);
            }
            itemUIComponent.countsCollected.text = countCollected.ToString();
        }

    }

    private void Update()
    {
        if (!startCountFlag) return;
        UpdateVisuals();
    }
}
