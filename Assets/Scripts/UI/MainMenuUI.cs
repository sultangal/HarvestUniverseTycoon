using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }

    [SerializeField] private GameObject group;
    [SerializeField] private GameObject enhanceGroup;
    [SerializeField] private GameObject swipeRaycastTarget;
    [SerializeField] private GameObject rotationRaycastTarget;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnShiftLeft;
    [SerializeField] private Button btnShiftRight;
    [SerializeField] private Button btnHarvSettings;
    [SerializeField] private Button btnBack;
    [SerializeField] private Transform textsGroup;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI cash;

    public event EventHandler OnStoreEnter;
    public event EventHandler OnBackToMainMenuFromStore;

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
            enhanceGroup.SetActive(false);
            btnPlay.gameObject.SetActive(false);
            swipeRaycastTarget.SetActive(false);
            btnBack.gameObject.SetActive(true);
            btnHarvSettings.gameObject.SetActive(false);
            btnShiftLeft.gameObject.SetActive(true);
            btnShiftRight.gameObject.SetActive(true);
            rotationRaycastTarget.SetActive(true);
            OnStoreEnter?.Invoke(this, EventArgs.Empty);
        });

        btnBack.onClick.AddListener(() =>
        {
            enhanceGroup.SetActive(true);
            btnPlay.gameObject.SetActive(true);
            swipeRaycastTarget.SetActive(true);
            btnBack.gameObject.SetActive(false);
            btnHarvSettings.gameObject.SetActive(true);
            btnShiftLeft.gameObject.SetActive(false);
            btnShiftRight.gameObject.SetActive(false);
            rotationRaycastTarget.SetActive(false);
            OnBackToMainMenuFromStore?.Invoke(this, EventArgs.Empty);
        });

        UpdateVisuals();
    }

    private void GameManager_OnCashAmountChanged(object sender, System.EventArgs e)
    {
        UpdateVisuals();
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

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            UpdateVisuals();
            group.SetActive(true);
        }
        else
        {
            group.SetActive(false);
        }
    }

    private void UpdateVisuals()
    {
        level.text = (GameManager.Instance.GlobalData_.level + 1).ToString();
        gold.text = GameManager.Instance.GlobalData_.amountOfGold.ToString();
        cash.text = "$ " + GameManager.Instance.GlobalData_.amountOfCash.ToString();
    }
}
