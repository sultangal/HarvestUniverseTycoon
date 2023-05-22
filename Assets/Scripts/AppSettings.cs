using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Debug.Log("target FPS before "+Application.targetFrameRate);
        Application.targetFrameRate = 60;
        Debug.Log("Screen refresh rate " + Screen.currentResolution.refreshRateRatio.value);
        Debug.Log("target FPS after " + Application.targetFrameRate);

    }
}
