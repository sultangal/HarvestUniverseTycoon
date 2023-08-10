using System;
using UnityEngine;

public class Countdown: MonoBehaviour 
{
    public event EventHandler OnTimeIsUp;

    private const float COUNTDOWN_TIME = 59.0f;
    private float countdownTime = COUNTDOWN_TIME;
    private bool countdownRunning = false;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    {
        Debug.Log("Game State is: " + GameManager.Instance.GetState());
        if (GameManager.Instance.IsGamePlaying())
        {
            StartCountdown();
        } else
        {
            StopCountdown();
        }
    }

    public bool SetCountdownTime(float time)
    {
        if (countdownRunning) return false;
        countdownTime = time;
        return true;
    }

    public float GetCountdownTime()
    {
        return countdownTime;
    }
    private void StartCountdown()
    {
        countdownTime = COUNTDOWN_TIME;
        countdownRunning = true;
    }
    private void StopCountdown()
    {
        countdownRunning = false;
    }

    private void Update()
    {
        if (countdownRunning)
        {
            countdownTime -= Time.deltaTime;
            if (countdownTime < 0)
            {
                OnTimeIsUp?.Invoke(this, EventArgs.Empty);
                countdownRunning = false;
                //Debug.Log("TIME IS UP!");
            }
        }
    }

}
