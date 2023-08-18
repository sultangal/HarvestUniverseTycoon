using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button btnCollect;
    [SerializeField] TextMeshProUGUI gameOver_UI;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        btnCollect.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameState(GameManager.GameState.WaitingToStart);
        });

    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            btnCollect.gameObject.SetActive(true);
            gameOver_UI.gameObject.SetActive(true);
        }
        else
        {
            btnCollect.gameObject.SetActive(false);
            gameOver_UI.gameObject.SetActive(false);
        }
    }
}
