using UnityEngine;
using UnityEngine.UI;

public class ModalWindowUI : MonoBehaviour
{
    [SerializeField] private Button ok;
    [SerializeField] private Button cancel;

    private void Start()
    {
        cancel.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
