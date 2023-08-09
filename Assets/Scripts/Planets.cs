using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Planets : MonoBehaviour
{
    public static Planets Instance { get; private set; }

    public PlanetSO[] planetsArr;
    [SerializeField] private PlanetMeshSO planetMeshSO;
    //public Vector3 CurrPlanetPosition { get; private set; };
    //public FieldItemSO CurrfieldItemSO { get; private set; } 

    public event EventHandler<OnPlanetShiftEventArgs> OnPlanetShift;

    public class OnPlanetShiftEventArgs : EventArgs
    {
        public Transform currPlanetTransform;
    }

    public int currentPlanetIndex = 0;
    private const float SPACE_BETWEEN_PLANETS = 15f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlanetsController!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CreatePlanets();
        SetPlanetsVisibility();
    }

    private void CreatePlanets()
    {
        for (int i = 0; i < planetsArr.Length; i++)
        {
            planetsArr[i].planetRef = Instantiate(
                planetMeshSO.planetMesh,
                planetMeshSO.planetMesh.position,
                planetMeshSO.planetMesh.rotation);
            
            Vector3 newPos = new(SPACE_BETWEEN_PLANETS * i, 0f, 0f);
            planetsArr[i].planetRef.position += newPos;
            planetsArr[i].planetRef.GetComponent<PlanetVisual>().SetPlanetColor(planetsArr[i].planetColor);
        }
    }

    public void ShiftPlanetLeft()
    {
        if (currentPlanetIndex > 0)
        {
            currentPlanetIndex--;
            SetPlanetsVisibility();
            //CurrPlanetPosition = planetsArr[currentPlanetIndex].planetRef.position;
            //CurrfieldItemSO = planetsArr[currentPlanetIndex].fieldItemSO;
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planetsArr[currentPlanetIndex].planetRef });
        }
    }

    public void ShiftPlanetRight()
    {
        if (currentPlanetIndex < planetsArr.Length-1)
        {
            currentPlanetIndex++;
            SetPlanetsVisibility();
            //CurrPlanetPosition = planetsArr[currentPlanetIndex].planetRef.position;
            //CurrfieldItemSO = planetsArr[currentPlanetIndex].fieldItemSO;
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planetsArr[currentPlanetIndex].planetRef });
        }
    }

    public Transform GetCurrentPlanet()
    {
        return planetsArr[currentPlanetIndex].planetRef;
    }

    private void SetPlanetsVisibility()
    {
        for (int i = 0; i < planetsArr.Length; i++)
        {
            planetsArr[i].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }

        if (currentPlanetIndex > 0)
        {
            planetsArr[currentPlanetIndex - 1].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }

        planetsArr[currentPlanetIndex].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = true;

        if (currentPlanetIndex < planetsArr.Length - 1)
        {
            planetsArr[currentPlanetIndex + 1].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    //private List<Transform> InstantiateFieldItems(FieldItemSO fieldItemSO, Vector3 planetPosition)
    //{
        //List<Transform> fieldItems = new();
        //foreach (var vertex in meshForPointsSource.vertices)
        //{
            //Vector3 position = new((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            //position *= randomMultiplier;
            //position += vertex;
            //position.Normalize();
            //position.Scale(new(5f, 5f, 5f));
            //Transform item = Instantiate(fieldItemSO.itemPrefab, position, Quaternion.LookRotation(position));
            //Vector3 turnItem = new(90.0f, 0.0f, 0.0f);
            //item.eulerAngles += turnItem;
            //item.Rotate(new(0.0f, (float)random.NextDouble() * 100, 0.0f));
            //item.position += planetPosition;
            //fieldItems.Add(item);
        //}
        //return fieldItems;
    //}
}
