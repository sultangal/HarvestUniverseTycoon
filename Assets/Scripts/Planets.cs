using System;
using UnityEngine;

public class Planets : MonoBehaviour
{
    public static Planets Instance { get; private set; }

    public PlanetSO[] planetsSOArr;
    [SerializeField] private PlanetMeshSO planetMeshSO;

    public event EventHandler<OnPlanetShiftEventArgs> OnPlanetShift;

    public class OnPlanetShiftEventArgs : EventArgs
    {
        public Transform currPlanetTransform;
        public float shiftSpeed;
        public bool isRight;
    }

    public PlanetData[] PlanetData;

    public int CurrentPlanetIndex { get; private set; } = 0;
    public int LastPlanetIndex { get; private set; } = 0;
    public readonly float SPACE_BETWEEN_PLANETS = 6f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Planets!");
            return;
        }
        Instance = this;
        InitializePlanetData();
        LoadSaves();
    }

    private void Start()
    {
        CurrentPlanetIndex = GameManager.Instance.GlobalData_.level;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        CreatePlanets();
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            MakeOnlyCurrPlanetVisible();
        }
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            MakeAllPlanetsVisible();
        }
    }

    private void CreatePlanets()
    { 
        LastPlanetIndex = planetsSOArr.Length-1;
        for (int i = 0; i < planetsSOArr.Length; i++)
        {
            planetsSOArr[i].planetPrefab = Instantiate(
                planetMeshSO.planetMesh,
                planetMeshSO.planetMesh.position,
                planetMeshSO.planetMesh.rotation);
            Vector3 newPos = new(SPACE_BETWEEN_PLANETS * i, 0f, 0f);
            planetsSOArr[i].planetPrefab.position += newPos;
            PlanetVisuals planetVisual = planetsSOArr[i].planetPrefab.GetComponent<PlanetVisuals>();
            planetVisual.planetId = i;
            planetVisual.SetPlanetMaterial(planetsSOArr[i].planetMaterial);
            planetVisual.SetPlanetColor(planetsSOArr[i].planetColor);
            if (i <= GameManager.Instance.GlobalData_.level)
                planetVisual.SetAvailabilityVisual(true); 
            else 
                planetVisual.SetAvailabilityVisual(false);    
        }
    }

    private void InitializePlanetData()
    {
        PlanetData = new PlanetData[planetsSOArr.Length];
        for (int i = 0; i < planetsSOArr.Length; i++)
        {
            PlanetData[i] = new PlanetData
            {
                amountOfCollectedFieldItemsOnPlanet = new int[planetsSOArr[i].fieldItemSOs.Length],
                goalAchievedFlags = new bool[planetsSOArr[i].fieldItemSOs.Length]
            };
        }
    }

    private void MakeOnlyCurrPlanetVisible()
    {
        for (int i = 0; i < planetsSOArr.Length; i++)
            SetVisibilityOfPlanet(i, false);
        SetVisibilityOfPlanet(CurrentPlanetIndex, true);
    }

    private void MakeAllPlanetsVisible()
    {
        for (int i = 0; i < planetsSOArr.Length; i++)
            SetVisibilityOfPlanet(i, true);
    }

    private void SetVisibilityOfPlanet(int index, bool visible)
    {
        planetsSOArr[index].planetPrefab.gameObject.SetActive(visible);
        //planetsArr[index].planetPrefab.gameObject.GetComponent<MeshRenderer>().enabled = visible;
        //planetsArr[index].planetPrefab.gameObject.GetComponent<Collider>().enabled = visible;
    }

    private void LoadSaves()
    {
        var result = SavingSystem.LoadPlanetDataFromFile();
        if (result != null)
        {
            PlanetData = result;       
        }

    }

    public void ShiftPlanetLeft()
    {
        ShiftPlanetLeft(1f);
    }

    public void ShiftPlanetLeft(float shiftSpeed)
    {
        if (CurrentPlanetIndex > 0)
        {
            CurrentPlanetIndex--;
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs 
            { 
                currPlanetTransform = planetsSOArr[CurrentPlanetIndex].planetPrefab,
                shiftSpeed = shiftSpeed,
                isRight = false
            });
        }
    }

    public void ShiftPlanetRight()
    {
        ShiftPlanetRight(1f);
    }

    public void ShiftPlanetRight(float shiftSpeed)
    {
        if (CurrentPlanetIndex < planetsSOArr.Length - 1)
        {
            CurrentPlanetIndex++;
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs
            {
                currPlanetTransform = planetsSOArr[CurrentPlanetIndex].planetPrefab,
                shiftSpeed = shiftSpeed,
                isRight = true
            });
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

    public Vector3 GetCurrentPlanetPosition()
    {
        return planetsSOArr[CurrentPlanetIndex].planetPrefab.transform.position;
    }

    public PlanetSO GetCurrentLevelPlanetSO()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level];
    }

    public int[] GetCurrentPlanetAmountOfCollectedItems()
    {
        return PlanetData[CurrentPlanetIndex].amountOfCollectedFieldItemsOnPlanet;
    }

    public int[] GetCurrentLevelAmountOfCollectedItems()
    {
        return PlanetData[GameManager.Instance.GlobalData_.level].amountOfCollectedFieldItemsOnPlanet;
    }

    public void AddCollectedAmountOfItems(int[] itemsCountArr)
    {
        var collectedItems = PlanetData[CurrentPlanetIndex].amountOfCollectedFieldItemsOnPlanet;
        
        for (var i = 0; i < collectedItems.Length; i++)
        {
            collectedItems[i] += itemsCountArr[i];

            if (collectedItems[i] >= planetsSOArr[CurrentPlanetIndex].fieldItemAmountGoal[i])
            {
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

    public bool IsCurrentPlanetAvailable()
    {
        if (CurrentPlanetIndex > GameManager.Instance.GlobalData_.level) 
            return false;
        else
            return true;
    }

    public bool IsFirstPlanet()
    {
        if (CurrentPlanetIndex == 0)
            return true;
        else
            return false;
    }

    public bool IsLastPlanet()
    {
        if (CurrentPlanetIndex == planetsSOArr.Length - 1)
            return true;
        else
            return false;
    }

    public int GetCurrLevelSpeedEnhanceCost()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level].speedEnhanceCost;
    }
    public int GetCurrLevelBladesEnhanceCost()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level].bladesEnhanceCost;
    }
    public int GetCurrLevelShieldEnhanceCost()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level].shieldEnhanceCost;
    }

    public float GetCurrLevelAsteriodMoveSpeed()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level].asteriodMoveSpeed;
    }

    public int GetCurrLevelMinSecBetweenAsteriodSpawn()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level].minSecBetweenSpawn;
    }

    public int GetCurrLevelMaxSecBetweenAsteriodSpawn()
    {
        return planetsSOArr[GameManager.Instance.GlobalData_.level].maxSecBetweenSpawn;
    }
}