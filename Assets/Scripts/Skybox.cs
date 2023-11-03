using System.Collections;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    [SerializeField] AnimationCurve animCurve;
    private Material skyboxMat;
    private float skyboxRotationCurr;
    private float skyboxRotationGoal;
    private readonly float ROTATION_AMOUNT = 1f;
    private bool rotateRightFlag;
    private bool rotateLeftFlag;
    private float time;
    private void Start()
    {
        Planets.Instance.OnPlanetShift += PlanetsController_OnPlanetShift;
        skyboxMat = RenderSettings.skybox;
        skyboxRotationCurr = 0f;
        rotateRightFlag = false;
        rotateLeftFlag = false;
        time = 0f;

    }


    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {
        if (e.isRight)
        {
            skyboxRotationGoal = skyboxRotationCurr + ROTATION_AMOUNT;
            //var keys = animCurve.keys;
            //keys[0].value = skyboxRotationCurr;
            //keys[1].value = skyboxRotationGoal;
            rotateRightFlag = true;           
        } else
        {
            skyboxRotationGoal = skyboxRotationCurr - ROTATION_AMOUNT;
            rotateLeftFlag = true;            
        }

    }

    private void Update()
    {
        if (rotateRightFlag)
        {
            RotateSkyboxRight();
        }
        else if (rotateLeftFlag) 
        {
            RotateSkyboxLeft();
        }
    }

    private void RotateSkyboxRight()
    {
        //time += Time.deltaTime;
        //skyboxRotationCurr = animCurve.Evaluate(time);
        skyboxRotationCurr += Time.deltaTime;
        skyboxMat.SetFloat("_Rotation", skyboxRotationCurr * 3);
        if (skyboxRotationCurr > skyboxRotationGoal) {
            time = 0f;
            rotateRightFlag = false;
        }
    }

    private void RotateSkyboxLeft()
    {
        skyboxRotationCurr -= Time.deltaTime;
        skyboxMat.SetFloat("_Rotation", skyboxRotationCurr * 3f);
        if (skyboxRotationCurr < skyboxRotationGoal)
        {
            time = 0f;
            rotateLeftFlag = false;
        }
    }
}
