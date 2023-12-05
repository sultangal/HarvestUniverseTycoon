using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }

    [SerializeField] private GameObject mainMenuGroup;
    [SerializeField] private GameObject storeGroup;
    [SerializeField] private GameObject headerGroup;
    [SerializeField] private GameObject enhancementsGroup;
    [SerializeField] private GameObject itemsGroup;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnHarvSettings;
    [SerializeField] private Button btnBack;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI cash;
    [SerializeField] private GameObject modalWindow;
    [SerializeField] private GameObject infoModalWindow;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one MainMenuUI!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        GameManager.Instance.OnCashAmountChanged += GameManager_OnCashAmountChanged;
        GameManager.Instance.OnGoldAmountChanged += GameManager_OnGoldAmountChanged;

        btnPlay.onClick.AddListener(() =>
        {
            if (Planets.Instance.IsCurrentPlanetAvailable())
            {
                btnPlay.interactable = true;
                DOTween.CompleteAll();
                GameManager.Instance.SetGameState(GameManager.GameState.GameSessionPlaying);
            } else
            {
                btnPlay.interactable = false;
            }
        });

        btnHarvSettings.onClick.AddListener(() =>
        {
            mainMenuGroup.SetActive(false);
            storeGroup.SetActive(true);
            enhancementsGroup.SetActive(false);
            itemsGroup.SetActive(false);
            StoreManager.Instance.FireOnStoreEnterEvent();   
        });

        btnBack.onClick.AddListener(() =>
        {
            mainMenuGroup.SetActive(true);
            storeGroup.SetActive(false);
            enhancementsGroup.SetActive(true);
            itemsGroup.SetActive(true);
            StoreManager.Instance.FireOnBackToMainMenuEvent();
        });

        UpdateHeader();
    }

    private void GameManager_OnGoldAmountChanged(object sender, EventArgs e)
    {
        UpdateHeader();
    }

    private void GameManager_OnCashAmountChanged(object sender, System.EventArgs e)
    {
        UpdateHeader();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            UpdateHeader();
            mainMenuGroup.SetActive(true);
            headerGroup.SetActive(true);
        }
        else
        {
            mainMenuGroup.SetActive(false);
            headerGroup.SetActive(false);
        }
    }

    private void UpdateHeader()
    {
        level.text = (GameManager.Instance.GlobalData_.level + 1).ToString();
        gold.text = GameManager.Instance.GlobalData_.amountOfGold.ToString();
        cash.text = "$ " + GameManager.Instance.GlobalData_.amountOfCash.ToString();
    }

    public bool UpdatePlayButtonAvailability()
    {
        if (Planets.Instance.IsCurrentPlanetAvailable())
        {
            btnPlay.interactable = true;
            btnHarvSettings.interactable = true;
            return true;
        }
        else
        {
            btnPlay.interactable = false;
            btnHarvSettings.interactable = false;
            return false;
        }
    }

    public void GenerateModalWindow(string text, Action onOKCallback)
    {
        var instance = Instantiate(modalWindow, this.transform);
        var mw_UI = instance.GetComponent<ModalWindowUI>();
        mw_UI.SetText(text);
        mw_UI.SetCallbackToOKButton(onOKCallback);
    }

    public void GenerateInfoModalWindow(string text)
    {
        var instance = Instantiate(infoModalWindow, this.transform);
        instance.GetComponent<InfoModalWindowUI>().SetText(text);
    }
}
