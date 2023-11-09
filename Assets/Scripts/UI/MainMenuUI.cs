using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnShiftLeft;
    [SerializeField] private Button btnShiftRight;
    [SerializeField] private Transform textsGroup;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI cash;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

        btnPlay.onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GameSessionPlaying);
        });

        btnShiftLeft.onClick.AddListener(() =>
        {
            Planets.Instance.ShiftPlanetLeft();
            SetButtonInteractivity();
        });

        btnShiftRight.onClick.AddListener(() =>
        {
            Planets.Instance.ShiftPlanetRight();
            SetButtonInteractivity();
        });
    }

    private void SetButtonInteractivity()
    {
        if (Planets.Instance.CurrentPlanetIndex <= GameManager.Instance.GlobalData_.level)        
            btnPlay.interactable = true;        
        else
            btnPlay.interactable = false;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            level.text = GameManager.Instance.GlobalData_.level.ToString();
            gold.text = GameManager.Instance.GlobalData_.amountOfGold.ToString();
            cash.text = "$ " + GameManager.Instance.GlobalData_.amountOfCash.ToString();
            btnPlay.gameObject.SetActive(true);
            btnShiftLeft.gameObject.SetActive(true);
            btnShiftRight.gameObject.SetActive(true);
            textsGroup.gameObject.SetActive(true);
        }
        else
        {
            btnPlay.gameObject.SetActive(false);
            btnShiftLeft.gameObject.SetActive(false);
            btnShiftRight.gameObject.SetActive(false);
            textsGroup.gameObject.SetActive(false);
        }
    }
}
