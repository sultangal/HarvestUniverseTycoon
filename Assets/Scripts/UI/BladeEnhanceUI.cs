using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BladeEnhanceUI : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI cost;
    public UnityEngine.UI.Image image;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            if (!HarvesterGroup.Instance.TryEnhanceBlades())
            {
                Debug.Log("You broke hahahaah!!");
            }
        });

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            cost.text = Planets.Instance.GetBladesEnhanceCost().ToString();
        }
    }
}
