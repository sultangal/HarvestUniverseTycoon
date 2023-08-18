using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnShiftLeft;
    [SerializeField] private Button btnShiftRight;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

        btnPlay.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GameSessionPlaying);
        });

        btnShiftLeft.GetComponent<Button>().onClick.AddListener(() =>
        {
            Planets.Instance.ShiftPlanetLeft();            
        });

        btnShiftRight.GetComponent<Button>().onClick.AddListener(() =>
        {
            Planets.Instance.ShiftPlanetRight();           
        });
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            btnPlay.gameObject.SetActive(true);
            btnShiftLeft.gameObject.SetActive(true);
            btnShiftRight.gameObject.SetActive(true);
        }
        else
        {
            btnPlay.gameObject.SetActive(false);
            btnShiftLeft.gameObject.SetActive(false);
            btnShiftRight.gameObject.SetActive(false);
        }
    }
}
