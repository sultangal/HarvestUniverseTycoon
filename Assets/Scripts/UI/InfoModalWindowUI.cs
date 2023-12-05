using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoModalWindowUI : MonoBehaviour
{
    [SerializeField] private Button ok;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        ok.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}
