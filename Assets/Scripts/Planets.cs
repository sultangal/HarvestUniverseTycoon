using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Planets : MonoBehaviour
{
    public static Planets Instance { get; private set; }

    [SerializeField] private PlanetSO[] planetsArr;
    [SerializeField] private PlanetMeshSO planetMeshSO;

    public event EventHandler<OnPlanetShiftEventArgs> OnPlanetShift;

    public class OnPlanetShiftEventArgs : EventArgs
    {
        public Transform currPlanetTransform;
    }

    public int CurrentPlanetIndex { get; private set; } = 0;
    private const float SPACE_BETWEEN_PLANETS = 15f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Planets!");
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
            planetsArr[i].planetPrefab = Instantiate(
                planetMeshSO.planetMesh,
                planetMeshSO.planetMesh.position,
                planetMeshSO.planetMesh.rotation);
            
            Vector3 newPos = new(SPACE_BETWEEN_PLANETS * i, 0f, 0f);
            planetsArr[i].planetPrefab.position += newPos;
            PlanetVisual planetVisual = planetsArr[i].planetPrefab.GetComponent<PlanetVisual>();
            planetVisual.planetId = i;
            planetVisual.SetPlanetColor(planetsArr[i].planetColor);
            if (i <= GameManager.Instance.GlobalData_.level)
                planetVisual.SetAvalability(true); 
            else 
                planetVisual.SetAvalability(false);    
        }
    }

    private void MakeCurrAndAdjasentPlanetsVisible()
    {
        MakeOnlyCurrPlanetVisible();
        if (CurrentPlanetIndex > 0)
            SetVisibilityOfPlanet(CurrentPlanetIndex - 1, true);
        if (CurrentPlanetIndex < planetsArr.Length - 1)
            SetVisibilityOfPlanet(CurrentPlanetIndex + 1, true);
    }

    private void MakeOnlyCurrPlanetVisible()
    {
        for (int i = 0; i < planetsArr.Length; i++)
            SetVisibilityOfPlanet(i, false);
        SetVisibilityOfPlanet(CurrentPlanetIndex, true);
    }

    private void SetVisibilityOfPlanet(int index, bool visible)
    {
        planetsArr[index].planetPrefab.gameObject.SetActive(visible);
        //planetsArr[index].planetPrefab.gameObject.GetComponent<MeshRenderer>().enabled = visible;
        //planetsArr[index].planetPrefab.gameObject.GetComponent<Collider>().enabled = visible;
    }

    public void ShiftPlanetLeft()
    {
        if (CurrentPlanetIndex > 0)
        {
            CurrentPlanetIndex--;
            MakeCurrAndAdjasentPlanetsVisible();
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs 
            { currPlanetTransform = planetsArr[CurrentPlanetIndex].planetPrefab });
        }
    }

    public void ShiftPlanetRight()
    {
        if (CurrentPlanetIndex < planetsArr.Length - 1)
        {
            CurrentPlanetIndex++;
            MakeCurrAndAdjasentPlanetsVisible();
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs 
            { currPlanetTransform = planetsArr[CurrentPlanetIndex].planetPrefab });
        }
    }

    public bool IsCurrPlanetActualLevel()
    {
        if (GameManager.Instance.GlobalData_.level == CurrentPlanetIndex)
            return true;
        else return false;
        
    }

    public PlanetSO GetCurrentPlanetSO()
    {
        return planetsArr[CurrentPlanetIndex];
    }
    public PlanetSO GetCurrentLevelPlanetSO()
    {
        return planetsArr[GameManager.Instance.GlobalData_.level];
    }
}
