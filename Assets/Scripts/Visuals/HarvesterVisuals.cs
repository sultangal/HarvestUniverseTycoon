using System.Collections.Generic;
using UnityEngine;

public class HarvesterVisuals : MonoBehaviour
{
    [SerializeField] private Transform harvesterBody;
    [SerializeField] private Transform harvesterBladesHolder;
    [SerializeField] private Transform harvesterBlades;
    [SerializeField] private Material unavailableMat;

    private List<Material> bodyMats;
    private List<Material> bladesHolderMats;
    private List<Material> bladesMats;

    private void Start()
    {
        bodyMats = new();
        bladesHolderMats = new();
        bladesMats = new();

        ScanHarvesterMaterials();
    }

    public void UpdateAvailabilityVisual()
    {
        if (Planets.Instance.IsCurrentPlanetAvailable())
        {
            SetMaterialToObject(harvesterBody, bodyMats);
            SetMaterialToObject(harvesterBladesHolder, bladesHolderMats);
            SetMaterialToObject(harvesterBlades, bladesMats);
        }
        else
        {
            SetMaterialToObject(harvesterBody, unavailableMat);
            SetMaterialToObject(harvesterBladesHolder, unavailableMat);
            SetMaterialToObject(harvesterBlades, unavailableMat);
        }
    }

    private void ScanHarvesterMaterials()
    {
        var comp0 = harvesterBody.GetComponent<MeshRenderer>();
        comp0.GetMaterials(bodyMats);

        var comp1 = harvesterBladesHolder.GetComponent<MeshRenderer>();
        comp1.GetMaterials(bladesHolderMats);

        var comp2 = harvesterBlades.GetComponent<MeshRenderer>();
        comp2.GetMaterials(bladesMats);
    }

    private void SetMaterialToObject(Transform gameObject, Material material)
    {
        List<Material> matList = new();
        var comp = gameObject.GetComponent<MeshRenderer>();
        comp.GetMaterials(matList);
        for (int i = 0; i < matList.Count; i++)        
            matList[i] = material;
        comp.SetMaterials(matList);
    }

    private void SetMaterialToObject(Transform gameObject, List<Material> materials)
    {
        List<Material> matList = new();
        var comp = gameObject.GetComponent<MeshRenderer>();
        comp.GetMaterials(matList);
        if (materials.Count != matList.Count) {
            Debug.LogError("Saved material list of harvester on start do not match with current collected list of materials");
            return;
        }
        for (int i = 0; i < matList.Count; i++)
            matList[i] = materials[i];
        comp.SetMaterials(matList);
    }

}
