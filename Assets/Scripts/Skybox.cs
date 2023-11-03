using DG.Tweening;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    [SerializeField] AnimationCurve animCurve;
    private Material skyboxMat;
    private readonly float ROTATION_MULT = 5f;
    private readonly float ANIM_DURATION = 1f;

    private void Start()
    {
        Planets.Instance.OnPlanetShift += PlanetsController_OnPlanetShift;
        skyboxMat = RenderSettings.skybox;
        skyboxMat.SetFloat("_Rotation", 0f);
    }

    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {
        if (e.isRight)                
            DOTween.To(
                () => skyboxMat.GetFloat("_Rotation"),
                x => skyboxMat.SetFloat("_Rotation", x),
                skyboxMat.GetFloat("_Rotation") + ROTATION_MULT,
                ANIM_DURATION);              
        else
            DOTween.To(
                () => skyboxMat.GetFloat("_Rotation"),
                x => skyboxMat.SetFloat("_Rotation", x),
                skyboxMat.GetFloat("_Rotation") - ROTATION_MULT,
                ANIM_DURATION);
    }

    private void OnDestroy()
    {
        skyboxMat.SetFloat("_Rotation", 0f);
    }
}
