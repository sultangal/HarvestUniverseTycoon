using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
            item.GetComponent<ItemUI>().countsGoal.text = "/" +
                Planets.Instance.GetCurrentLevelPlanetSO().fieldItemAmountForNextLevel[i].ToString();
        }
    }

    private void UpdateVisuals()
    {
        for (int i = 0; i < GameManager.Instance.GameSessionData_.CollectedFieldItems.Length; i++)
        {
            int countCollected = GameManager.Instance.GameSessionData_.CollectedFieldItems[i];
            int countsGoal = Planets.Instance.GetCurrentLevelPlanetSO().fieldItemAmountForNextLevel[i];
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
