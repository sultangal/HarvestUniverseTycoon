using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnShiftLeft;
    [SerializeField] private Button btnShiftRight;
    [SerializeField] private Countdown countdown;
    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;

        btnPlay.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GameSessionPlaying);
            countdown.StartCountdown();
            btnPlay.gameObject.SetActive(false);
            btnShiftLeft.gameObject.SetActive(false);
            btnShiftRight.gameObject.SetActive(false);
        });

        btnShiftLeft.GetComponent<Button>().onClick.AddListener(() =>
        {
            PlanetsController.Instance.ShiftPlanetLeft();            
        });

        btnShiftRight.GetComponent<Button>().onClick.AddListener(() =>
        {
            PlanetsController.Instance.ShiftPlanetRight();           
        });
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameSessionEnded())
        {
            btnPlay.gameObject.SetActive(true);
            btnShiftLeft.gameObject.SetActive(true);
            btnShiftRight.gameObject.SetActive(true);
        }
    }
}
