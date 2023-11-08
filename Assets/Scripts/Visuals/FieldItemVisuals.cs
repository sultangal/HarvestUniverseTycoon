using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldItemVisuals : MonoBehaviour
{
    [SerializeField] private Transform itemPlane;
    public Color ColorForVehicleParticles;
    private bool isAvailable = false;
    private Material itemMaterial;


    public void SetAvailabilityVisual(bool isAvalable)
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
        List<Material> matList = new List<Material>();
        var comp = itemPlane.GetComponent<MeshRenderer>();
        comp.GetMaterials(matList);
        foreach ( Material mat in matList ) 
            mat.SetColor("_BaseColor", new Color(0.0f, 0.0f, 0.0f));
        

        //itemMaterial.SetColor("_EmissionColor", new Color(0.5f, 0.5f, 0.5f));
        //planetMaterial.SetColor("_FresnelColor", new Color(0.0f, 0.0f, 0.0f));
    }

    private void ApplyColorToPlanet()
    {
        List<Material> matList = new List<Material>();
        var comp = itemPlane.GetComponent<MeshRenderer>();
        comp.GetMaterials(matList);
        foreach (Material mat in matList)
            mat.SetColor("_BaseColor", new Color(0.443f, 0.443f, 0.443f));
        //itemMaterial.SetColor("_Emission", new Color(0.0f, 0.0f, 0.0f));
        //planetMaterial.SetColor("_FresnelColor", planetColor);
    }

}
