using System;
using UnityEngine;

public class PlanetVisual : MonoBehaviour
{
    public int planetId;
    private bool isAvailable = false;
    private Color planetColor;
    private Material planetMaterial;

    private void Start()
    {
        GameManager.Instance.OnLevelUp += GameManager_OnLevelUp;
    }

    private void GameManager_OnLevelUp(object sender, GameManager.OnOnLevelUpEventArgs e)
    {
       if (planetId == e.level) ApplyColorToPlanet();
    }

    public void SetPlanetColor(Color color)
    {
        planetColor = color;
    }

    public void SetAvalabilityVisual(bool isAvalable)
    {
        isAvailable = isAvalable;
        if (isAvailable)
        {
            ApplyColorToPlanet();
        }
        else
        {
            ApplyGreyToPlanet();
        }
    }

    private void ApplyGreyToPlanet()
    {
        planetMaterial = GetComponent<Renderer>().material;
        planetMaterial.SetColor("_BaseColor", new Color(0.0f, 0.0f, 0.0f));
        planetMaterial.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
        //planetMaterial.SetColor("_FresnelColor", new Color(0.0f, 0.0f, 0.0f));
    }

    private void ApplyColorToPlanet()
    {
        planetMaterial = GetComponent<Renderer>().material;
        planetMaterial.SetColor("_BaseColor", planetColor);
        planetMaterial.SetColor("_Emission", new Color(0.0f, 0.0f, 0.0f));
        //planetMaterial.SetColor("_FresnelColor", planetColor);
    }

}
