using DG.Tweening;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    [SerializeField] AnimationCurve animCurve;
    private Material skyboxMat;
    private readonly float ROTATION_MULT = 5f;
    private readonly float ANIM_DURATION = 1f;

    private Color colorAvailable;
    private Color colorNotAvailable;

    private void Start()
    {
        colorAvailable = Color.gray;
        colorNotAvailable = new(0.1f, 0.1f, 0.1f);

        Planets.Instance.OnPlanetShift += PlanetsController_OnPlanetShift;
        skyboxMat = RenderSettings.skybox;
        skyboxMat.SetFloat("_Rotation", 0f);
    }

    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {      
        if (e.isRight)
        {
            DOTween.To(
                () => skyboxMat.GetFloat("_Rotation"),
                x => skyboxMat.SetFloat("_Rotation", x),
                skyboxMat.GetFloat("_Rotation") + ROTATION_MULT,
                ANIM_DURATION);

        }
        else
        {
            DOTween.To(
                () => skyboxMat.GetFloat("_Rotation"),
                x => skyboxMat.SetFloat("_Rotation", x),
                skyboxMat.GetFloat("_Rotation") - ROTATION_MULT,
                ANIM_DURATION);
        }

        SetSkyboxTint();
    }

    private void SetSkyboxTint()
    {
        if (Planets.Instance.IsCurrentPlanetAvailable())
            DOTween.To(
                () => skyboxMat.GetColor("_Tint"),
                x => skyboxMat.SetColor("_Tint", x),
                colorAvailable,
                ANIM_DURATION);
        else
            DOTween.To(
                () => skyboxMat.GetColor("_Tint"),
                x => skyboxMat.SetColor("_Tint", x),
                colorNotAvailable,
                ANIM_DURATION);
    }

    private void OnDestroy()
    {
        skyboxMat.SetFloat("_Rotation", 0f);
        skyboxMat.SetColor("_Tint", colorAvailable);
    }
}
