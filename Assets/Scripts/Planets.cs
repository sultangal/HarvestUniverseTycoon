using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Planets : MonoBehaviour
{
    public static Planets Instance { get; private set; }

    [SerializeField] private PlanetSO[] planetsSOArr;
    [SerializeField] private PlanetMeshSO planetMeshSO;

    public event EventHandler<OnPlanetShiftEventArgs> OnPlanetShift;

    public class OnPlanetShiftEventArgs : EventArgs
    {
        public Transform currPlanetTransform;
    }

    public PlanetData[] PlanetData { get; private set; }

    public int CurrentPlanetIndex { get; private set; } = 0;
    public int LastPlanetIndex { get; private set; } = 0;
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
        PlanetData = new PlanetData[planetsSOArr.Length];
        LastPlanetIndex = planetsSOArr.Length-1;
        for (int i = 0; i < planetsSOArr.Length; i++)
        {
            planetsSOArr[i].planetPrefab = Instantiate(
                planetMeshSO.planetMesh,
                planetMeshSO.planetMesh.position,
                planetMeshSO.planetMesh.rotation);

            PlanetData[i] = new PlanetData
            {
                amountOfCollectedFieldItemsOnPlanet = new int[planetsSOArr[i].fieldItemSOs.Length],
                goalAchievedFlags = new bool[planetsSOArr[i].fieldItemSOs.Length]
            };

            Vector3 newPos = new(SPACE_BETWEEN_PLANETS * i, 0f, 0f);
            planetsSOArr[i].planetPrefab.position += newPos;
            PlanetVisual planetVisual = planetsSOArr[i].planetPrefab.GetComponent<PlanetVisual>();
            planetVisual.planetId = i;
            planetVisual.SetPlanetColor(planetsSOArr[i].planetColor);
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
        if (CurrentPlanetIndex < planetsSOArr.Length - 1)
            SetVisibilityOfPlanet(CurrentPlanetIndex + 1, true);
    }

    private void MakeOnlyCurrPlanetVisible()
    {
        for (int i = 0; i < planetsSOArr.Length; i++)
            SetVisibilityOfPlanet(i, false);
        SetVisibilityOfPlanet(CurrentPlanetIndex, true);
    }

    private void SetVisibilityOfPlanet(int index, bool visible)
    {
        planetsSOArr[index].planetPrefab.gameObject.SetActive(visible);
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
            { currPlanetTransform = planetsSOArr[CurrentPlanetIndex].planetPrefab });
        }
    }

    public void ShiftPlanetRight()
    {
        if (CurrentPlanetIndex < planetsSOArr.Length - 1)
        {
            CurrentPlanetIndex++;
            MakeCurrAndAdjasentPlanetsVisible();
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs 
            { currPlanetTransform = planetsSOArr[CurrentPlanetIndex].planetPrefab });
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
        return planetsSOArr[CurrentPlanetIndex];
    }
    public PlanetSO GetCurrentLevelPlanetSO()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level];
    }

    public int[] GetCurrentPlanetAmountOfCollectedItems()
    {
        return PlanetData[CurrentPlanetIndex].amountOfCollectedFieldItemsOnPlanet;
    }

    public void AddCollectedAmountOfItems(int[] itemsCountArr)
    {
        //if (!Planets.Instance.IsCurrPlanetActualLevel()) return;
        //if (itemsCountArr.Length != AmountOfCollectedFieldItemsOnPlanet.Length)
        //{
        //    Debug.LogError("Array length does't the same.");
        //    return;
        //}

        var collectedItems = PlanetData[CurrentPlanetIndex].amountOfCollectedFieldItemsOnPlanet;
        
        for (var i = 0; i < collectedItems.Length; i++)
        {
            collectedItems[i] += itemsCountArr[i];

            if (collectedItems[i] > planetsSOArr[CurrentPlanetIndex].fieldItemAmountForNextLevel[i])
            {
                //collectedItems[i] = itemAmountForNextLevel;
                PlanetData[CurrentPlanetIndex].goalAchievedFlags[i] = true;
            }

        }

    }

    public bool CheckIfNextLevelGoalAchieved()
    {
        foreach (var flag in PlanetData[CurrentPlanetIndex].goalAchievedFlags)
        {
            if (!flag)
                return false;
        }
        return true;
    }
}
