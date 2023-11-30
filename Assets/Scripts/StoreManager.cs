using DG.Tweening;
using System;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }

    public HarvestersSO[] harvestersPrefabRefs;
    private bool[] harvPrefabUnlocked;
    private readonly float SPACE_BETWEEN_HARV = 3f;
    private int currentPrefabIndex;

    public event EventHandler<OnUpdateHarvesterPrefabArgs> OnUpdateHarvesterPrefab;
    public event EventHandler OnStoreEnter;
    public event EventHandler OnBackToMainMenu;

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
        currentPrefabIndex = 1;

        InitializeUnlockedArray();
        CheckIfIndexIsValid();
        CreateHarvesters();
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
            harvestersPrefabRefs[i].harvesterSceneRefPrefab.transform.SetParent(this.transform);
            harvestersPrefabRefs[i].harvesterSceneRefPrefab.transform.position += newPos;

            if (i != currentPrefabIndex)
                harvestersPrefabRefs[i].harvesterSceneRefPrefab.SetActive(false);
            harvestersPrefabRefs[i].harvesterSceneRefPrefab.GetComponent<HarvesterVisuals>().SetPivotToMenuMode();
        }
        //position group to current harv
        this.transform.position = new(-SPACE_BETWEEN_HARV * currentPrefabIndex, 0f, 0f);
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

    private void StartDOMove()
    {
        var positionX = Planets.Instance.GetCurrentPlanetPosition().x - currentPrefabIndex * SPACE_BETWEEN_HARV;
        this.transform.DOMove(new(positionX, 0f, 0f), 1f);
    }

    public void ShiftLeft()
    {
        if (currentPrefabIndex > 0)
        {
            currentPrefabIndex--;
            var transform = harvestersPrefabRefs[currentPrefabIndex].harvesterSceneRefPrefab.transform;
            OnUpdateHarvesterPrefab?.Invoke(this, new OnUpdateHarvesterPrefabArgs
            {
                prefab = transform
            });
            StartDOMove();
        }
    }

    public void ShiftRight()
    {
        if (currentPrefabIndex < harvestersPrefabRefs.Length - 1)
        {
            currentPrefabIndex++;
            var transform = harvestersPrefabRefs[currentPrefabIndex].harvesterSceneRefPrefab.transform;
            OnUpdateHarvesterPrefab?.Invoke(this, new OnUpdateHarvesterPrefabArgs
            {
                prefab = transform
            });
            StartDOMove();
        }
    }

    public void FireOnStoreEnterEvent()
    {
        foreach (var item in harvestersPrefabRefs)
        {
            var prefab = item.harvesterSceneRefPrefab;
            prefab.SetActive(true);
            prefab.GetComponent<Rotation>().enabled = false;
        }
        OnStoreEnter?.Invoke(this, EventArgs.Empty);
    }

    public void FireOnBackToMainMenuEvent()
    {
        for (int i = 0; i < harvestersPrefabRefs.Length; i++)
        {
            var prefab = harvestersPrefabRefs[i].harvesterSceneRefPrefab;
            prefab.GetComponent<Rotation>().enabled = false;
            if (i!=currentPrefabIndex)
                prefab.SetActive(false);
        }
        OnBackToMainMenu?.Invoke(this, EventArgs.Empty);
    }
}
