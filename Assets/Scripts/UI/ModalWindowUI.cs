using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModalWindowUI : MonoBehaviour
{
    [SerializeField] private Button ok;
    [SerializeField] private Button cancel;
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        cancel.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    public void SetCallbackToOKButton(Action callback)
    {
        ok.onClick.AddListener(() =>
        {
            callback();
            Destroy(gameObject);
        });
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }
}