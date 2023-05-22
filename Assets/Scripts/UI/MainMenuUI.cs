using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button btnPlay;
    // Start is called before the first frame update
    private void Start()
    {
        btnPlay.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.SetGameState(GameManager.GameState.GamePlaying);
            btnPlay.gameObject.SetActive(false);
        });

    }
}
