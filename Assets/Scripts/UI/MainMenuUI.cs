using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
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
        });
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameSessionEnded())
        {
            btnPlay.gameObject.SetActive(true);
        }
    }
}
