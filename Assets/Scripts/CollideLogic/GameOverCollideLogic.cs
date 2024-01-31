using UnityEngine;

public class GameOverCollideLogic : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged()
    {
        //if (GameManager.Instance.IsGamePlaying())
        //    gameObject.SetActive(true);
        //else
        //    gameObject.SetActive(false);

        if (!GameManager.Instance.IsGamePlaying())
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject);
        GameManager.Instance.SetGameState(GameManager.GameState.GameOver);        
    }
}
