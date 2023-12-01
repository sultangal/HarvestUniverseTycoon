using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    //[SerializeField] private Button back;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button buy;
    [SerializeField] private TextMeshProUGUI tmp_buy;
    [SerializeField] private GameObject modalWindow;
    [SerializeField] private Button mw_OK;


    private void Start()
    {

        buy.onClick.AddListener(() =>
        {
            modalWindow.SetActive(true);
        });

        left.onClick.AddListener(() =>
        {
            StoreManager.Instance.ShiftLeft();
            CheckPrefabAvailability();

        });

        right.onClick.AddListener(() =>
        {
            StoreManager.Instance.ShiftRight();
            CheckPrefabAvailability();
        });

        mw_OK.onClick.AddListener(() =>
        {
            if (StoreManager.Instance.BuyHarvester())
            {
                modalWindow.SetActive(false);
                buy.gameObject.SetActive(false);
            }
        });
    }

    private void OnEnable()
    {
        CheckPrefabAvailability();
    }

    private void CheckPrefabAvailability()
    {
        if (!StoreManager.Instance.IsCurrPrefabAvailable())
        {
            buy.gameObject.SetActive(true);
            ShowThePrice();
        }
        else
        {
            buy.gameObject.SetActive(false);
        }
    }

    private void ShowThePrice()
    {
        tmp_buy.text = StoreManager.Instance.GetHarvPrice().ToString();
    }

}
