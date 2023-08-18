using TMPro;
using UnityEngine;

public class PlayingUI : MonoBehaviour
{
    [SerializeField] FloatingJoystick joystick_UI;
    [SerializeField] TextMeshProUGUI score_UI;
    [SerializeField] TextMeshProUGUI countdown_UI;

    private void Start()
    {
        GameManager.Instance.OnCashAmountChanged += GameManager_OnScoreChanged;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            joystick_UI.gameObject.SetActive(true);
            score_UI.gameObject.SetActive(true);
            countdown_UI.gameObject.SetActive(true);
        } else
        {
            joystick_UI.gameObject.SetActive(false);
            score_UI.gameObject.SetActive(false);
            countdown_UI.gameObject.SetActive(false);
        }      
    }

    private void GameManager_OnScoreChanged(object sender, System.EventArgs e)
    {
        score_UI.GetComponent<TextMeshProUGUI>().text = "$" + GameManager.Instance.GetCashAmount().ToString();
    }

    private void Update()
    {
        countdown_UI.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(GameManager.Instance.CountdownTime).ToString() + "sec";
    }

}
