using DG.Tweening;
using System;
using UnityEngine;

public class Skybox : MonoBehaviour
{
    public static Skybox Instance { get; private set; }

    private Material skyboxMat;
    private readonly float ROTATION_MULT = 5f;
    private float currRotation;

    private Color colorAvailable;
    private Color colorNotAvailable;
    private int currIndex;
    private readonly float DURATION = 0.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Skybox!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currRotation = 0f;
        colorAvailable = Color.white;
        colorNotAvailable = new(0.3f, 0.3f, 0.3f);
        currIndex = Planets.Instance.GetCurrentLevelPlanetSO().skyboxIndex;

        Planets.Instance.OnPlanetShift += PlanetsController_OnPlanetShift;
        skyboxMat = RenderSettings.skybox;
        skyboxMat.SetFloat("_Rotation", 0f);
        for (int i = 0; i <= 10; i++)
        {
            skyboxMat.SetFloat("_BlendCubemaps" + i.ToString(), 0.0f);
        }
        skyboxMat.SetFloat("_BlendCubemaps" + currIndex.ToString(), 1.0f);

    }

    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {      
        if (e.isRight)
        {
            currRotation += ROTATION_MULT;
            DOTween.To(
                () => skyboxMat.GetFloat("_Rotation"),
                x => skyboxMat.SetFloat("_Rotation", x),
                skyboxMat.GetFloat("_Rotation") + ROTATION_MULT,
                e.shiftSpeed).SetId(10);
        }
        else
        {
            currRotation -= ROTATION_MULT;
            DOTween.To(
                () => skyboxMat.GetFloat("_Rotation"),
                x => skyboxMat.SetFloat("_Rotation", x),
                skyboxMat.GetFloat("_Rotation") - ROTATION_MULT,
                e.shiftSpeed).SetId(10);           
        }
        SetSkybox(Planets.Instance.GetCurrentPlanetSO().skyboxIndex);
        SetSkyboxTint();
    }

    private void SetSkyboxTint()
    {
        if (Planets.Instance.IsCurrentPlanetAvailable())
            DOTween.To(
                () => skyboxMat.GetColor("_Tint"),
                x => skyboxMat.SetColor("_Tint", x),
                colorAvailable,
                DURATION);
        else
            DOTween.To(
                () => skyboxMat.GetColor("_Tint"),
                x => skyboxMat.SetColor("_Tint", x),
                colorNotAvailable,
                DURATION);
    }

    private void SetSkybox(int index)
    {       
        if (index == currIndex) return;
        if (index > currIndex)
        {
            DOTween.Sequence()
            .Append(
                DOTween.To(
                    () => skyboxMat.GetFloat("_BlendCubemaps" + index.ToString()),
                    x => skyboxMat.SetFloat("_BlendCubemaps" + index.ToString(), x),
                    1.0f,
                    DURATION))
            .AppendCallback(
                () => skyboxMat.SetFloat("_BlendCubemaps" + currIndex.ToString(), 0.0f))
                .AppendCallback(
                    () => currIndex = index);
        }
        else
        {
            DOTween.Sequence()
                .AppendCallback(
                    () => skyboxMat.SetFloat("_BlendCubemaps" + index.ToString(), 1.0f))
                .Append(
                    DOTween.To(
                        () => skyboxMat.GetFloat("_BlendCubemaps" + currIndex.ToString()),
                        x => skyboxMat.SetFloat("_BlendCubemaps" + currIndex.ToString(), x),
                        0.0f,
                        DURATION))
                .AppendCallback(
                    () => currIndex = index);
        }
    }

    public void ReturnRotation()
    {
        DOTween.To(
            () => skyboxMat.GetFloat("_Rotation"),
            x => skyboxMat.SetFloat("_Rotation", x),
            currRotation,
            1f).SetId(10);
    }

    private void OnDestroy()
    {
        skyboxMat.SetFloat("_Rotation", 0f);
        skyboxMat.SetColor("_Tint", colorAvailable);
        skyboxMat.SetFloat("_BlendCubemaps0", 1.0f);
        for (int i = 1; i <= 10; i++)
        {
            skyboxMat.SetFloat("_BlendCubemaps" + i.ToString(), 0.0f);
        }
    }
}
