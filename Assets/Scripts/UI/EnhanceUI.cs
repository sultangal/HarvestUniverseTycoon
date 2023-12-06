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
    [SerializeField] private Image unlocked;
    private readonly Color imageTint = new (0f, 0.69f, 0f);

    private bool isUnlockedflag;

    public enum EnhanceType
    {
        Blades, Speed, Shield
    }
    public EnhanceType enhanceType;

    private void Start()
    {
        isUnlockedflag = false;
        button.onClick.AddListener(() =>
        {
            SetUnlocked(true);
            InitiateEnhance();
        });
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        GameManager.Instance.OnCashAmountChanged += GameManager_OnCashAmountChanged;
        Planets.Instance.OnPlanetShift += Planets_OnPlanetShift;
        InitializeUI();
    }

    private void Planets_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {
        if (Planets.Instance.IsCurrentPlanetAvailable())
            UpdateAvailableVisibility();
        else
            SetAvailable(false);
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
            GameManager.Instance.OnCashAmountChanged += GameManager_OnCashAmountChanged;
            InitializeUI();
        }
        else
        {
            GameManager.Instance.OnCashAmountChanged -= GameManager_OnCashAmountChanged;
            cost.gameObject.SetActive(false);
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            if (isUnlockedflag)
            {
                StartCountdown();
            }
            else
            {
                SetAvailable(false);
            }           
        }
    }

    private void InitializeUI()
    {
        cost.gameObject.SetActive(true);
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
            image.color = imageTint;
            cost.color = Color.green;
        }
        else
        {
            button.interactable = false;
            image.color = imageTint * Color.gray;
            cost.color = Color.gray;
        }
    }

    private void SetUnlocked(bool isVisible)
    {
        if (isVisible)
        {
            button.enabled = false;
            unlocked.enabled = true;
            isUnlockedflag = true;
        }
        else
        {
            button.enabled = true;
            unlocked.enabled = false;
            unlocked.fillAmount = 1.0f;
            isUnlockedflag = false;
        }
    }

    private int GetEnhanceCost()
    {
        if (enhanceType == EnhanceType.Blades)
        {
            return Planets.Instance.GetCurrLevelBladesEnhanceCost();
        } 
        else if (enhanceType == EnhanceType.Speed)
        {
            return Planets.Instance.GetCurrLevelSpeedEnhanceCost();
        } 
        else if (enhanceType == EnhanceType.Shield)
        {
            return Planets.Instance.GetCurrLevelShieldEnhanceCost();
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
            SpeedEnhanceControl.Instance.TryEnhanceSpeed();
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
            var instance = BladesEnhanceControl.Instance;                     
            instance.StartBladesCountdown();
            instance.callbackVisuals = IterateCountdownAnim;
        }
        else if (enhanceType == EnhanceType.Speed)
        {
            var instance = SpeedEnhanceControl.Instance;
            instance.StartCountdown();
            instance.callbackVisuals = IterateCountdownAnim;
        }
        else if (enhanceType == EnhanceType.Shield)
        {
            var instance = ShieldEnhanceControl.Instance;
            instance.StartShieldCountdown();
            instance.callbackVisuals = IterateCountdownAnim;
        }
    }

    private void IterateCountdownAnim(float iterate, float duration, bool isRunning)
    {
        if (isRunning)
        {
            iterate /= duration;
            unlocked.fillAmount -= iterate;
        } else
        {
            SetAvailable(false);
        }
    }
}
