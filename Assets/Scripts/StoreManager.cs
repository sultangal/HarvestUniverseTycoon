using DG.Tweening;
using System;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public static StoreManager Instance { get; private set; }

    public HarvestersSO[] harvestersPrefabRefs;
    public bool[] harvPrefabUnlocked { get; private set; }
    private int currAvailablePrefabIndex;
    private int currPrefabIndex;

    public event EventHandler<OnUpdateHarvesterPrefabArgs> OnUpdateHarvesterPrefab;
    public event EventHandler OnStoreEnter;
    public event EventHandler OnBackToMainMenu;
    
    private readonly float SPACE_BETWEEN_HARV = 3f;

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
        currAvailablePrefabIndex = 1;
        currPrefabIndex = 1;

        InitializeUnlockedArray();
        CheckIfIndexIsValid();
        CreateHarvesters();
    }

    private void InitializeUnlockedArray()
    {
        harvPrefabUnlocked = new bool[harvestersPrefabRefs.Length];
        harvPrefabUnlocked[0] = true;
        harvPrefabUnlocked[1] = true;
        harvPrefabUnlocked[2] = false;
    }

    private void CreateHarvesters()
    {
        for (int i = 0; i < harvestersPrefabRefs.Length; i++)
        {
             GameObject prefab = Instantiate(
                harvestersPrefabRefs[i].harvesterPrefab,
                Vector3.zero,
                Quaternion.identity);
            Vector3 newPos = new(SPACE_BETWEEN_HARV * i, 5.381f, 0.726f);
            prefab.transform.SetParent(this.transform);
            prefab.transform.position += newPos;

            if (i != currAvailablePrefabIndex)
                prefab.SetActive(false);
            prefab.GetComponent<HarvesterVisuals>().SetPivotToMenuMode();

            if (harvPrefabUnlocked[i] == false)
                prefab.GetComponent<HarvesterVisuals>().SetAvailability(false);

            harvestersPrefabRefs[i].harvesterSceneRefPrefab = prefab;
        }
        //position group to current harv
        this.transform.position = new(-SPACE_BETWEEN_HARV * currAvailablePrefabIndex, 0f, 0f);
    }

    private bool CheckIfIndexIsValid()
    {
        if (harvPrefabUnlocked[currAvailablePrefabIndex] == true)
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
        var positionX = Planets.Instance.GetCurrentPlanetPosition().x - currAvailablePrefabIndex * SPACE_BETWEEN_HARV;
        this.transform.DOMove(new(positionX, 0f, 0f), 1f).SetId(20);
    }

    public Transform GetCurrentPrefab()
    {
        return harvestersPrefabRefs[currAvailablePrefabIndex].harvesterSceneRefPrefab.transform;
    }

    public bool IsCurrPrefabAvailable()
    {
        return harvPrefabUnlocked[currAvailablePrefabIndex] == true;
    }

    public float EvaluateCurrAvailablePrefabPosX()
    {
        return currAvailablePrefabIndex * SPACE_BETWEEN_HARV;
    }

    public float GetHarvPrice()
    {
        return harvestersPrefabRefs[currAvailablePrefabIndex].price;
    }

    public void ShiftLeft()
    {
        if (currAvailablePrefabIndex > 0)
        {
            currAvailablePrefabIndex--;
            var transform = harvestersPrefabRefs[currAvailablePrefabIndex].harvesterSceneRefPrefab.transform;
            OnUpdateHarvesterPrefab?.Invoke(this, new OnUpdateHarvesterPrefabArgs
            {
                prefab = transform
            });
            StartDOMove();
        }
    }

    public void ShiftRight()
    {
        if (currAvailablePrefabIndex < harvestersPrefabRefs.Length - 1)
        {
            currAvailablePrefabIndex++;
            var transform = harvestersPrefabRefs[currAvailablePrefabIndex].harvesterSceneRefPrefab.transform;
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
        currPrefabIndex = currAvailablePrefabIndex;
        OnStoreEnter?.Invoke(this, EventArgs.Empty);
    }

    public void FireOnBackToMainMenuEvent()
    {
        DOTween.Complete(20);
        if (!IsCurrPrefabAvailable())
        {
            currAvailablePrefabIndex = currPrefabIndex;
            this.transform.localPosition = new(-currAvailablePrefabIndex * SPACE_BETWEEN_HARV, 0f, 0f);
            //this.transform.rotation = Quaternion.identity;
            //this.transform.localScale = Vector3.one;

            OnUpdateHarvesterPrefab?.Invoke(this, new OnUpdateHarvesterPrefabArgs
            {
                prefab = GetCurrentPrefab()
            });
        }

        for (int i = 0; i < harvestersPrefabRefs.Length; i++)
        {
            var prefab = harvestersPrefabRefs[i].harvesterSceneRefPrefab;
            prefab.GetComponent<Rotation>().enabled = true;
            if (i != currAvailablePrefabIndex)
                prefab.SetActive(false);
        }

        OnBackToMainMenu?.Invoke(this, EventArgs.Empty);
    }

    public bool BuyHarvester()
    {
        if (GameManager.Instance.TryWithdrawGold(harvestersPrefabRefs[currAvailablePrefabIndex].price))
        {
            harvPrefabUnlocked[currAvailablePrefabIndex] = true;
            GetCurrentPrefab().GetComponent<HarvesterVisuals>().SetAvailability(true);
            return true;
        }
        else
        {
            Debug.Log("Not enough gold!");
            return false;
        }
    }
}
