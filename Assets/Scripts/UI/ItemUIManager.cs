using UnityEngine;
using System.Collections;
using TMPro;


public class ItemUIManager : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject group;
    [SerializeField] private Transform itemUITemplate;
    private bool startCountFlag = false;
    private readonly float Y_POSITION_GAMEPLAY = 757.69f;
    private readonly float Y_POSITION_MENU = 495f;
    private Color achieved;

    private void Start()
    {
        achieved = new(0.0f, 1.0f, 0.0f);
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        StoreManager.Instance.OnStoreEnter += MainMenuUI_OnStoreEnter;
        StoreManager.Instance.OnBackToMainMenu += MainMenuUI_OnBackToMainMenu;
        ReinitializeMenuVisuals();
    }

    private void MainMenuUI_OnBackToMainMenu(object sender, System.EventArgs e)
    {
        group.SetActive(true);
    }

    private void MainMenuUI_OnStoreEnter(object sender, System.EventArgs e)
    {
        group.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            ReinitializeGameplayVisuals();
            startCountFlag = true;
        }
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsTimeIsUp())
        {
            startCountFlag = false;
        }

        if (GameManager.Instance.IsGameWaitingToStart())
        {
            ReinitializeMenuVisuals();
        }

    }

    private void ResetItems()
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    private void ReinitializeGameplayVisuals()
    {
        ResetItems();
        var planetSO = Planets.Instance.GetCurrentPlanetSO();
        container.localPosition = new(0f, Y_POSITION_GAMEPLAY, 0f);
        for (int i = 0; i < planetSO.fieldItemAmountGoal.Length; i++)
        {
            var item = Instantiate(itemUITemplate, container);
            var itemUI = item.GetComponent<ItemUI>();
            itemUI.countsGoal.text = "/" +
                planetSO.fieldItemAmountGoal[i].ToString();
            itemUI.itemImage.sprite =
                planetSO.fieldItemSOs[i].itemSprite;
        }
    }

    private void ReinitializeMenuVisuals()
    {
        ResetItems();
        var planetInst = Planets.Instance;
        var planetSO = planetInst.GetCurrentLevelPlanetSO();
        container.localPosition = new(0f, Y_POSITION_MENU, 0f);
        for (int i = 0; i < planetSO.fieldItemAmountGoal.Length; i++)
        {
            var item = Instantiate(itemUITemplate, container);
            var itemUI = item.GetComponent<ItemUI>();
            int amountGoal = planetSO.fieldItemAmountGoal[i];
            itemUI.countsGoal.text = "/" +
                amountGoal.ToString();
            int amount = planetInst.GetCurrentLevelAmountOfCollectedItems()[i];
            if (amount >= amountGoal)
            {
                itemUI.countsCollected.color = achieved;
            }
            itemUI.countsCollected.text = amount.ToString();
            itemUI.itemImage.sprite =
                planetSO.fieldItemSOs[i].itemSprite;
        }
    }

    private void UpdateGameplayVisuals()
    {
        var gm = GameManager.Instance;
        var pln = Planets.Instance;
        for (int i = 0; i < gm.GameSessionData_.CollectedFieldItems.Length; i++)
        {
            int countCollected =
                pln.GetCurrentPlanetAmountOfCollectedItems()[i] + gm.GameSessionData_.CollectedFieldItems[i];
            //int countCollected = gm.GameSessionData_.CollectedFieldItems[i];
            int countsGoal = pln.GetCurrentPlanetSO().fieldItemAmountGoal[i];
            var itemUI = container.GetChild(i).GetComponent<ItemUI>();
            if (countCollected >= countsGoal)
            {
                itemUI.countsCollected.color = achieved;
            }
            itemUI.countsCollected.text = countCollected.ToString();
        }

    }

    private void Update()
    {
        if (!startCountFlag) return;
        UpdateGameplayVisuals();
    }
}
