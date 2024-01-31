using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeIsUpUI : MonoBehaviour
{
    [SerializeField] private Button btnCollect;
    [SerializeField] TextMeshProUGUI timeIsUp_UI;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        btnCollect.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameState(GameManager.GameState.WaitingToStart);
        });
    }

    private void GameManager_OnGameStateChanged()
    {
        if (GameManager.Instance.IsTimeIsUp())
        {
            btnCollect.gameObject.SetActive(true);
            timeIsUp_UI.gameObject.SetActive(true);
        }
        else
        {
            btnCollect.gameObject.SetActive(false);
            timeIsUp_UI.gameObject.SetActive(false);
        }
    }
}
