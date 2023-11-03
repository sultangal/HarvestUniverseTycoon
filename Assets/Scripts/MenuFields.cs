using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFields : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            gameObject.SetActive(true);
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            gameObject.SetActive(false);
        }
    }
}
