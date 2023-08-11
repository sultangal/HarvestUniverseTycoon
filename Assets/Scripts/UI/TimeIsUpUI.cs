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
            btnCollect.gameObject.SetActive(false);
            timeIsUp_UI.gameObject.SetActive(false);
        });
        btnCollect.gameObject.SetActive(false);
        timeIsUp_UI.gameObject.SetActive(false);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsTimeIsUp())
        {
            btnCollect.gameObject.SetActive(true);
            timeIsUp_UI.gameObject.SetActive(true);
        }
    }
}
