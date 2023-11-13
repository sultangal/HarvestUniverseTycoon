using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BladeEnhanceUI : MonoBehaviour
{
    [SerializeField] private GameObject bladeGroup;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Image image;

    private void Start()
    {
        button.onClick.AddListener(() =>
        {
            HarvesterGroup.Instance.TryEnhanceBlades();
            UpdateBladeEnhanceAvailability();
        });

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        UpdateBladeEnhanceAvailability();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            bladeGroup.SetActive(true);
            UpdateBladeEnhanceAvailability();
            cost.text = Planets.Instance.GetBladesEnhanceCost().ToString();
        } else
            bladeGroup.SetActive(false);
    }

    private void UpdateBladeEnhanceAvailability()
    {
        if (GameManager.Instance.GlobalData_.amountOfCash >= Planets.Instance.GetBladesEnhanceCost())
            SetButtonInteractivity(true);
        else
            SetButtonInteractivity(false);
    }

    private void SetButtonInteractivity(bool isInteractable)
    {
        if (isInteractable)
        {
            button.interactable = true;
            image.color = Color.white;
            cost.color = Color.green;
        }
        else
        {
            button.interactable = false;
            image.color = Color.gray;
            cost.color = Color.gray;
        }
    }
}
