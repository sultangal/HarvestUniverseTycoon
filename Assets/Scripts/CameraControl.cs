using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Camera cameraRef;
    private const float MIN_ASPECT_RATIO = 0.5625f;
    private void Start()
    {
        //cameraRef = GetComponent<Camera>();
#if !UNITY_EDITOR
        float aspectRatio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        Debug.Log("Game View Aspect Ratio: " + aspectRatio);
        if (aspectRatio < MIN_ASPECT_RATIO)
        {
            /*this formula was calculated with mathway, to find correlation
            between aspectRatio and fieldOfView to keep planet in screen borders 
            for different screens*/
            cam.fieldOfView = -100.79892481f * aspectRatio + 112.1993952f;
        }
        else
        {
            cam.fieldOfView = 55.5f;
        }
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
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }   
#endif




}
