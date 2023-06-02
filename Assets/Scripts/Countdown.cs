using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countdown: MonoBehaviour 
{
    public event EventHandler OnTimeIsUp;

    private const float COUNTDOWN_TIME = 59.0f;
    private float countdownTime = COUNTDOWN_TIME;
    private bool countdownRunning = false;

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
    public void StartCountdown()
    {
        countdownTime = COUNTDOWN_TIME;
        countdownRunning = true;
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
