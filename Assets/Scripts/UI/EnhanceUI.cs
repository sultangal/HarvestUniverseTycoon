using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnhanceUI : MonoBehaviour
{
    [SerializeField] private GameObject group;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private Image image;

    private Outline outline;
    private bool isUnlockedflag;

    public enum EnhanceType
    {
        Blades, Speed, Shield
    }
    public EnhanceType enhanceType;

    private void Start()
    {
        outline = button.gameObject.GetComponent<Outline>();      
        button.onClick.AddListener(() =>
        {
            SetUnlocked(true);
            InitiateEnhance();
        });

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        GameManager.Instance.OnCashAmountChanged += GameManager_OnCashAmountChanged;
        InitializeUI();
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
            InitializeUI();
        }
        else
            group.SetActive(false);

        if (GameManager.Instance.IsGamePlaying() && isUnlockedflag)
        {
            StartCountdown();
        }
    }

    private void InitializeUI()
    {
        group.SetActive(true);
        SetUnlocked(false);
        UpdateAvailableVisibility();
        cost.text = GetEnhanceCost().ToString();
    }

    private void UpdateAvailableVisibility()
    {
        if (GameManager.Instance.GlobalData_.amountOfCash >= GetEnhanceCost())
            SetAvailable(true);
        else
            SetAvailable(false);
    }

    private void SetAvailable(bool isAvailable)
    {
        if (isAvailable)
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

    private void SetUnlocked(bool isVisible)
    {
        if (isVisible)
        {
            button.enabled = false;
            outline.enabled = true;
            isUnlockedflag = true;
        }
        else
        {
            button.enabled = true;
            outline.enabled = false;
            isUnlockedflag = false;
        }
    }

    private int GetEnhanceCost()
    {
        if (enhanceType == EnhanceType.Blades)
        {
            return Planets.Instance.GetBladesEnhanceCost();
        } 
        else if (enhanceType == EnhanceType.Speed)
        {
            return Planets.Instance.GetSpeedEnhanceCost();
        } 
        else if (enhanceType == EnhanceType.Shield)
        {
            return Planets.Instance.GetShieldEnhanceCost();
        }
        return int.MaxValue;
    }

    private void InitiateEnhance()
    {
        if (enhanceType == EnhanceType.Blades)
        {
            BladesEnhanceControl.Instance.TryEnhanceBlades();
        }
        else if (enhanceType == EnhanceType.Speed)
        {
            HarvesterMovementControl.Instance.TryEnhanceSpeed();
        }
        else if (enhanceType == EnhanceType.Shield)
        {
            ShieldEnhanceControl.Instance.TryShiledEnhance();
        }
    }

    private void StartCountdown()
    {
        if (enhanceType == EnhanceType.Blades)
        {
            BladesEnhanceControl.Instance.StartBladesCountdown();
        }
        else if (enhanceType == EnhanceType.Speed)
        {
            HarvesterMovementControl.Instance.StartSpeedCountdown();
        }
        else if (enhanceType == EnhanceType.Shield)
        {
            ShieldEnhanceControl.Instance.StartCountdown();
        }
    }
}
