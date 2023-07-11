using System.Collections;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float timeToAjust = 2.0f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float horizVelocity = 0.0f;
    private float vertVelocity = 0.0f;

    [SerializeField] private Camera cameraRef;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] AnimationCurve cameraCurve;
    private const float MIN_ASPECT_RATIO = 0.5625f;
    private void Start()
    {
        accelRatePerSec = 1f / timeToAjust;
        decelRatePerSec = -1f / timeToAjust;
        //cameraRef = GetComponent<Camera>();
#if !UNITY_EDITOR
        float aspectRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        Debug.Log("Game View Aspect Ratio: " + aspectRatio);
        if (aspectRatio < MIN_ASPECT_RATIO)
        {
            /*this formula was calculated with mathway, to find correlation
            between aspectRatio and fieldOfView to keep planet in screen borders 
            for different screens*/
            cameraRef.fieldOfView = -100.79892481f * aspectRatio + 112.1993952f;
        }
        else
        {
            cameraRef.fieldOfView = 55.5f;
        }

        CameraMotion();
#endif
    }

#if UNITY_EDITOR
    private void Update()
    {
        Vector2 gameViewSize = GetMainGameViewSize();
        float aspectRatio = gameViewSize.x / gameViewSize.y;
        //Debug.Log("Game View Aspect Ratio: " + aspectRatio);
        if (aspectRatio < MIN_ASPECT_RATIO)
        {
            /*this formula was calculated with mathway, to find correlation
            between aspectRatio and fieldOfView to keep planet in screen borders 
            for different screens*/
            cameraRef.fieldOfView = -100.79892481f * aspectRatio + 112.1993952f;
        }
        else
        {
            cameraRef.fieldOfView = 55.5f;
        }

        CameraMotion();
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }   
#endif


    private void CameraMotion()
    {

        if (joystick.Vertical > 0f)
        {
            horizVelocity += accelRatePerSec * Time.deltaTime;
            horizVelocity = Mathf.Min(horizVelocity, 1f);         
        }
        else if (joystick.Vertical < 0f)
        {
            horizVelocity += decelRatePerSec * Time.deltaTime;
            horizVelocity = Mathf.Max(horizVelocity, 0f);
        }

        if (joystick.Horizontal < 0f)
        {
            vertVelocity += accelRatePerSec * Time.deltaTime;
            vertVelocity = Mathf.Min(vertVelocity, 1f);
        }
        else if (joystick.Horizontal > 0f)
        {
            vertVelocity += decelRatePerSec * Time.deltaTime;
            vertVelocity = Mathf.Max(vertVelocity, 0f);
        }

        transform.localEulerAngles = new(cameraCurve.Evaluate(horizVelocity), 0f, cameraCurve.Evaluate(vertVelocity));

        // Debug.Log("cameraCurve.Evaluate(forwardHorizVelocity) " + cameraCurve.Evaluate(forwardHorizVelocity));
    }

}
