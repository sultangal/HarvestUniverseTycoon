using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance { get; private set; }

    [SerializeField] private GameObject group;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnShiftLeft;
    [SerializeField] private Button btnShiftRight;
    [SerializeField] private Transform textsGroup;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI cash;

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

        btnShiftLeft.onClick.AddListener(() =>
        {
            Planets.Instance.ShiftPlanetLeft();
            UpdatePlayButtonAvailability();
        });

        btnShiftRight.onClick.AddListener(() =>
        {
            Planets.Instance.ShiftPlanetRight();
            UpdatePlayButtonAvailability();
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
            return true;
        }
        else
        {
            btnPlay.interactable = false;
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
