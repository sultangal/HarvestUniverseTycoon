using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Planets : MonoBehaviour
{
    public static Planets Instance { get; private set; }

    [SerializeField] private PlanetSO[] planetsArr;
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
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        CreatePlanets();
        MakeCurrAndAdjasentPlanetsVisible();
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            MakeOnlyCurrPlanetVisible();
        }
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

    private void MakeCurrAndAdjasentPlanetsVisible()
    {
        MakeOnlyCurrPlanetVisible();
        if (currentPlanetIndex > 0)
            SetVisibilityOfPlanet(currentPlanetIndex - 1, true);
        if (currentPlanetIndex < planetsArr.Length - 1)
            SetVisibilityOfPlanet(currentPlanetIndex + 1, true);
    }

    private void MakeOnlyCurrPlanetVisible()
    {
        for (int i = 0; i < planetsArr.Length; i++)
            SetVisibilityOfPlanet(i, false);
        SetVisibilityOfPlanet(currentPlanetIndex, true);
    }

    private void SetVisibilityOfPlanet(int index, bool visible)
    {
        //planetsArr[index + 1].planetRef.gameObject.SetActive(gameObject.activeSelf);
        planetsArr[index].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = visible;
    }

    public void ShiftPlanetLeft()
    {
        if (currentPlanetIndex > 0)
        {
            currentPlanetIndex--;
            MakeCurrAndAdjasentPlanetsVisible();
            //CurrPlanetPosition = planetsArr[currentPlanetIndex].planetRef.position;
            //CurrfieldItemSO = planetsArr[currentPlanetIndex].fieldItemSO;
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planetsArr[currentPlanetIndex].planetRef });
        }
    }

    public void ShiftPlanetRight()
    {
        if (currentPlanetIndex < planetsArr.Length - 1)
        {
            currentPlanetIndex++;
            MakeCurrAndAdjasentPlanetsVisible();
            //CurrPlanetPosition = planetsArr[currentPlanetIndex].planetRef.position;
            //CurrfieldItemSO = planetsArr[currentPlanetIndex].fieldItemSO;
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planetsArr[currentPlanetIndex].planetRef });
        }
    }

    public PlanetSO GetCurrentPlanetSO()
    {
        return planetsArr[currentPlanetIndex];
    }
}
