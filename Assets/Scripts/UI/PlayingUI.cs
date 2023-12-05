using TMPro;
using UnityEngine;

public class PlayingUI : MonoBehaviour
{
    [SerializeField] FloatingJoystick joystick_UI;
    //[SerializeField] TextMeshProUGUI cashe_UI;
    [SerializeField] TextMeshProUGUI gold_UI;
    [SerializeField] TextMeshProUGUI countdown_UI;

    private void Start()
    {
        //GameManager.Instance.OnCashAmountChanged += GameManager_OnScoreChanged;
        GameManager.Instance.OnGoldAmountChanged += GameManager_OnGoldAmountChanged;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGoldAmountChanged(object sender, System.EventArgs e)
    {
        gold_UI.GetComponent<TextMeshProUGUI>().text = GameManager.Instance.GetGoldAmount().ToString();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            joystick_UI.gameObject.SetActive(true);
            //cashe_UI.gameObject.SetActive(true);
            gold_UI.gameObject.SetActive(true);
            countdown_UI.gameObject.SetActive(true);
        } else
        {
            joystick_UI.gameObject.SetActive(false);
            //cashe_UI.gameObject.SetActive(false);
            gold_UI.gameObject.SetActive(false);
            countdown_UI.gameObject.SetActive(false);
        }      
    }
    /*
    private void GameManager_OnScoreChanged(object sender, System.EventArgs e)
    {
        cashe_UI.GetComponent<TextMeshProUGUI>().text = "$ " + GameManager.Instance.GetCashAmount().ToString();
    }*/

    private void Update()
    {
        countdown_UI.GetComponent<TextMeshProUGUI>().text = Mathf.Ceil(GameManager.Instance.CountdownTime).ToString();
    }

}
