using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    //[SerializeField] private Button back;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button price;
    [SerializeField] private TextMeshProUGUI tmp_price;

    private void Start()
    {
        price.onClick.AddListener(() =>
        {
            MainMenuUI.Instance.GenerateModalWindow("Buy new harvester ?", Store.Instance.BuyHarvester);
        });

        left.onClick.AddListener(() =>
        {
            Store.Instance.ShiftLeft();
            CheckPrefabAvailability();
        });

        right.onClick.AddListener(() =>
        {
            Store.Instance.ShiftRight();
            CheckPrefabAvailability();
        });
    }

    private void OnEnable()
    {
        CheckPrefabAvailability();
    }

    private void CheckPrefabAvailability()
    {
        if (!Store.Instance.IsCurrPrefabAvailable())
        {
            price.gameObject.SetActive(true);
            ShowThePrice();
        }
        else
        {
            price.gameObject.SetActive(false);
        }
    }

    private void ShowThePrice()
    {
        tmp_price.text = Store.Instance.GetHarvPrice().ToString();
    }

    public void SetPriceButtonAvailability(bool isAvailable)
    {
        if (isAvailable)
        {
            price.gameObject.SetActive(true);
        }
        else
        {
            price.gameObject.SetActive(false);
        }
    }

}
