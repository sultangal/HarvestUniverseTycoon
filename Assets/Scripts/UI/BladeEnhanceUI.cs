using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BladeEnhanceUI : MonoBehaviour
{
    [SerializeField] private GameObject bladeGroup;
    [SerializeField] private Button bladeButton;
    [SerializeField] private TextMeshProUGUI bladeCost;
    [SerializeField] private Image bladeImage;
    [SerializeField] private float enhanceTimeSec;
    private Outline outline;
    private bool isUnlockedflag;
    private bool startCountdown;
    private float timeCountdown;

    private void Start()
    {
        outline = bladeButton.gameObject.GetComponent<Outline>();
        outline.enabled = false;
        isUnlockedflag = false;
        ResetCountdown();
        bladeButton.onClick.AddListener(() =>
        {
            SetUnlocked(true);
            HarvesterGroup.Instance.TryEnhanceBlades();       
                          
        });

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        GameManager.Instance.OnCashAmountChanged += GameManager_OnCashAmountChanged;
        UpdateAvailableVisibility();
    }

    private void GameManager_OnCashAmountChanged(object sender, System.EventArgs e)
    {
        if (!isUnlockedflag)
            UpdateAvailableVisibility();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            ResetCountdown();
            bladeGroup.SetActive(true);
            SetUnlocked(false);
            UpdateAvailableVisibility();
            bladeCost.text = Planets.Instance.GetBladesEnhanceCost().ToString();
        } else
            bladeGroup.SetActive(false);

        if (GameManager.Instance.IsGamePlaying() && isUnlockedflag)
        {
            startCountdown = true;
        }
    }

    private void UpdateAvailableVisibility()
    {
        if (GameManager.Instance.GlobalData_.amountOfCash >= Planets.Instance.GetBladesEnhanceCost())
            SetAvailable(true);
        else
            SetAvailable(false);
    }

    private void SetAvailable(bool isAvailable)
    {
        if (isAvailable)
        {
            bladeButton.interactable = true;
            bladeImage.color = Color.white;
            bladeCost.color = Color.green;
        }
        else
        {
            bladeButton.interactable = false;
            bladeImage.color = Color.gray;
            bladeCost.color = Color.gray;
        }
    }

    private void SetUnlocked(bool isVisible)
    {
        if (isVisible)
        {
            bladeButton.enabled = false;
            outline.enabled = true;
            isUnlockedflag = true;
        }
        else
        {
            bladeButton.enabled = true;
            outline.enabled = false;
            isUnlockedflag = false;
        }
    }

    private void ResetCountdown()
    {
        startCountdown = false;
        timeCountdown = enhanceTimeSec;
    }

    private void Update()
    {
        if (startCountdown)
        {
            timeCountdown -= Time.deltaTime;
            if (timeCountdown <= 0)
            {
                startCountdown = false;
                HarvesterGroup.Instance.DehanceBlades();

            }
        }
    }
}
