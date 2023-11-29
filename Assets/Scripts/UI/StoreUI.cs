using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private Button back;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button buy;
    [SerializeField] private GameObject modalWindow;

    private void Start()
    {
        buy.onClick.AddListener(() =>
        {
            modalWindow.SetActive(true);
        });
    }
}
