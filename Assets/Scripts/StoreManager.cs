using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }

    [SerializeField] private Transform harvestersGroup;
    public HarvestersSO[] harvestersPrefabRefs;
    private bool[] harvPrefabUnlocked;

    private readonly float SPACE_BETWEEN_HARV = 3f;

    public EventHandler<OnUpdateHarvesterPrefabArgs> OnUpdateHarvesterPrefab;

    private int currentPrefabIndex;

    public class OnUpdateHarvesterPrefabArgs : EventArgs
    {
        public Transform prefab;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one HarvestersStore!");
            return;
        }
        Instance = this;
        InitializeUnlockedArray();
        CheckIfIndexIsValid();
        CreateHarvesters();
        currentPrefabIndex = 2;
    }

    private void Start()
    {
        
        //CreateHarvesters();
        //OnUpdateHarvesterPrefabArgs args = new()
        //{
        //    prefab = currentPrefab
        //};
        //OnUpdateHarvesterPrefab?.Invoke(this, args);
    }

    private void InitializeUnlockedArray()
    {
        harvPrefabUnlocked = new bool[harvestersPrefabRefs.Length];
        harvPrefabUnlocked[0] = true;
        harvPrefabUnlocked[1] = true;
        harvPrefabUnlocked[2] = true;
    }

    public Transform GetCurrentPrefab()
    {
        return harvestersPrefabRefs[currentPrefabIndex].harvesterSceneRefPrefab.transform;
    }

    private void CreateHarvesters()
    {
        for (int i = 0; i < harvestersPrefabRefs.Length; i++)
        {
            harvestersPrefabRefs[i].harvesterSceneRefPrefab = Instantiate(
                harvestersPrefabRefs[i].harvesterPrefab,
                Vector3.zero,
                Quaternion.identity);
            Vector3 newPos = new(SPACE_BETWEEN_HARV * i, 5.381f, 0.726f);
            harvestersPrefabRefs[i].harvesterSceneRefPrefab.transform.SetParent(harvestersGroup);
            harvestersPrefabRefs[i].harvesterSceneRefPrefab.transform.position += newPos;

            //if (i != currentPrefabIndex)
            //    harvestersPrefabRefs[i].harvesterSceneRefPrefab.SetActive(false);
            

           // PlanetData[i] = new PlanetData
           // {
           //     amountOfCollectedFieldItemsOnPlanet = new int[planetsSOArr[i].fieldItemSOs.Length],
           //     goalAchievedFlags = new bool[planetsSOArr[i].fieldItemSOs.Length]
           // };
           //
           // Vector3 newPos = new(SPACE_BETWEEN_PLANETS * i, 0f, 0f);
           // planetsSOArr[i].planetPrefab.position += newPos;
           // PlanetVisuals planetVisual = planetsSOArr[i].planetPrefab.GetComponent<PlanetVisuals>();
           // planetVisual.planetId = i;
           // planetVisual.SetPlanetMaterial(planetsSOArr[i].planetMaterial);
           // planetVisual.SetPlanetColor(planetsSOArr[i].planetColor);
           // if (i <= GameManager.Instance.GlobalData_.level)
           //     planetVisual.SetAvalabilityVisual(true);
           // else
           //     planetVisual.SetAvalabilityVisual(false);
        }
    }

    private bool CheckIfIndexIsValid()
    {
        if (harvPrefabUnlocked[currentPrefabIndex] == true)
        {
            return true;
        }
        else
        {
            Debug.LogError("CurrentPrefabIndex does not match with available harvPrefabUnlocked list. You've made a mistake.");
            return false;
        }
    }

}
