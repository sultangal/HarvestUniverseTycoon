using DG.Tweening;
using System;
using UnityEngine;

public class Store : MonoBehaviour
{
    public static Store Instance { get; private set; }

    [SerializeField] private StoreUI store_UI;
    [SerializeField] private Transform harvesterGroup;

    public HarvestersSO[] harvestersPrefabRefs;
    public bool[] HarvestersBought { get; private set; }
    public int currAvailablePrefabIndex;
    private int prefabIndexInStore;

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

        prefabIndexInStore = 0;
        InitializeUnlockedArray();
        CheckIfIndexIsValid();
        CreateHarvesters();
    }

    private void InitializeUnlockedArray()
    {
        HarvestersBought = SavingSystem.LoadHarvesterStoreData(ref currAvailablePrefabIndex);
        if (HarvestersBought == null)
        {
            HarvestersBought = new bool[harvestersPrefabRefs.Length];
            HarvestersBought[0] = true;
            currAvailablePrefabIndex = 0;
        }
    }

    private void CreateHarvesters()
    {
        for (int i = 0; i < harvestersPrefabRefs.Length; i++)
        {
             GameObject prefab = Instantiate(
                harvestersPrefabRefs[i].harvesterPrefab,
                Vector3.zero,
                Quaternion.identity);
            var StartPos = MenuControl.PREFAB_START_POS;

            Vector3 newPos = new(SPACE_BETWEEN_HARV * i, StartPos.y, StartPos.z);
            prefab.transform.SetParent(harvesterGroup);
            prefab.transform.position += newPos;

            if (i != currAvailablePrefabIndex)
                prefab.SetActive(false);
            prefab.GetComponent<HarvesterVisuals>().SetPivotToMenuMode();

            if (HarvestersBought[i] == false)
                prefab.GetComponent<HarvesterVisuals>().SetAvailability(false);
            harvestersPrefabRefs[i].harvesterSceneRefPrefab = prefab;
        }
        //position group to current harv
        harvesterGroup.position = new(-SPACE_BETWEEN_HARV * currAvailablePrefabIndex, 0f, 0f);

    }

    private bool CheckIfIndexIsValid()
    {
        if (HarvestersBought[currAvailablePrefabIndex] == true)
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
        harvesterGroup.DOMove(new(positionX, 0f, 0f), 1f).SetId(20);
    }

    public Transform GetCurrentPrefab()
    {
        return harvestersPrefabRefs[currAvailablePrefabIndex].harvesterSceneRefPrefab.transform;
    }

    public bool IsCurrPrefabAvailable()
    {
        return HarvestersBought[currAvailablePrefabIndex] == true;
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
        prefabIndexInStore = currAvailablePrefabIndex;
        OnStoreEnter?.Invoke(this, EventArgs.Empty);
    }

    public void FireOnBackToMainMenuEvent()
    {
        DOTween.Complete(20);
        if (!IsCurrPrefabAvailable())
        {
            currAvailablePrefabIndex = prefabIndexInStore;
            harvesterGroup.localPosition = new(-currAvailablePrefabIndex * SPACE_BETWEEN_HARV, 0f, 0f);
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
        SavingSystem.SaveGame();
        OnBackToMainMenu?.Invoke(this, EventArgs.Empty);
    }

    public void BuyHarvester()
    {
        if (GameManager.Instance.TryWithdrawGold(harvestersPrefabRefs[currAvailablePrefabIndex].price))
        {
            HarvestersBought[currAvailablePrefabIndex] = true;
            SavingSystem.SaveGame();
            GetCurrentPrefab().GetComponent<HarvesterVisuals>().SetAvailability(true);
            store_UI.SetPriceButtonAvailability(false);
        }
        else
        {
            MainMenuUI.Instance.GenerateInfoModalWindow("Not enough gold!");
        }
    }
}
