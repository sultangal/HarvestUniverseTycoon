using System;
using System.Collections.Generic;
using UnityEngine;

public class FieldItemVisuals : MonoBehaviour
{
    [SerializeField] private Transform itemPlane;
    public Color ColorForVehicleParticles;
    private bool isAvailable = false;

    public void SetAvailabilityVisual(bool isAvalable)
    {
        isAvailable = isAvalable;
        if (isAvailable)        
            SetBaseColorToObjectsMat(itemPlane, new(0.443f, 0.443f, 0.443f));        
        else       
            SetBaseColorToObjectsMat(itemPlane, Color.black);        
    }

    private void SetBaseColorToObjectsMat(Transform gameObject, Color color)
    {
        List<Material> matList = new();
        var comp = gameObject.GetComponent<MeshRenderer>();
        comp.GetMaterials(matList);
        foreach (Material mat in matList)
        {
            mat.SetColor("_BaseColor", color);
        }
    }
}
