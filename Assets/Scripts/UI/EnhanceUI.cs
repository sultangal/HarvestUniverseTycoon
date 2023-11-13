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
        Blades, Speed, Something
    }
    public EnhanceType enhanceType;

    private void Start()
    {
        outline = button.gameObject.GetComponent<Outline>();
        outline.enabled = false;
        isUnlockedflag = false;
        button.onClick.AddListener(() =>
        {
            SetUnlocked(true);
            InitiateEnhance();
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
            group.SetActive(true);
            SetUnlocked(false);
            UpdateAvailableVisibility();
            cost.text = GetEnhanceCost().ToString();
        }
        else
            group.SetActive(false);

        if (GameManager.Instance.IsGamePlaying() && isUnlockedflag)
        {
            HarvesterGroup.Instance.StartSpeedCountdown();
        }
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
        else if (enhanceType == EnhanceType.Something)
        {
            throw new NotImplementedException();
        }
        return int.MaxValue;
    }

    private void InitiateEnhance()
    {
        if (enhanceType == EnhanceType.Blades)
        {
            HarvesterGroup.Instance.TryEnhanceBlades();
        }
        else if (enhanceType == EnhanceType.Speed)
        {
            HarvesterGroup.Instance.TryEnhanceSpeed();
        }
        else if (enhanceType == EnhanceType.Something)
        {
            throw new NotImplementedException();
        }
    }

    private void StartCountdown()
    {
        if (enhanceType == EnhanceType.Blades)
        {
            HarvesterGroup.Instance.StartBladesCountdown();
        }
        else if (enhanceType == EnhanceType.Speed)
        {
            HarvesterGroup.Instance.StartSpeedCountdown();
        }
        else if (enhanceType == EnhanceType.Something)
        {
            throw new NotImplementedException();
        }
    }
}
