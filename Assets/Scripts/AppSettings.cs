using UnityEngine;

public class AppSettings : MonoBehaviour
{
    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        //Debug.Log("target FPS before " + Application.targetFrameRate);
        Application.targetFrameRate = -1;
        //Debug.Log("Screen refresh rate " + Screen.currentResolution.refreshRateRatio.value);
        //Debug.Log("target FPS after " + Application.targetFrameRate);
        //Debug.Log(Screen.currentResolution);
    }


}
