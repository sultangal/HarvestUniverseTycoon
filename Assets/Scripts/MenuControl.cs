using DG.Tweening;
using System;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public static MenuControl Instance { get; private set; }

    [SerializeField] private Transform harvestersGroup;
    [SerializeField] private Transform harvesterMainMenuParent;
    [SerializeField] private Transform gameplayParent;

    private Transform harvesterPrefab;
    private static readonly Vector3 PREFAB_DEFAULT_POS = new(0.0f, 5.381f, 0.726f);

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one HarvesterGroup!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Planets.Instance.OnPlanetShift += PlanetsController_OnPlanetShift;
        MainMenuUI.Instance.OnStoreEnter += MainMenuUI_OnStoreEnter;
        MainMenuUI.Instance.OnBackToMainMenuFromStore += MainMenuUI_OnBackToMainMenuFromStore;
        StoreManager.Instance.OnUpdateHarvesterPrefab += HarvestersStore_OnUpdateHarvesterPrefab;

        harvesterPrefab = StoreManager.Instance.GetCurrentPrefab();
        harvestersGroup.SetParent(harvesterMainMenuParent);//
        HarvesterAppearence();
    }

    private void HarvestersStore_OnUpdateHarvesterPrefab(object sender , StoreManager.OnUpdateHarvesterPrefabArgs e)
    {
        harvesterPrefab = e.prefab;
    }


    private void MainMenuUI_OnBackToMainMenuFromStore(object sender, System.EventArgs e)
    {
        harvesterMainMenuParent.GetComponent<Rotation>().enabled = true;
    }

    private void MainMenuUI_OnStoreEnter(object sender, System.EventArgs e)
    {
        harvesterMainMenuParent.GetComponent<Rotation>().enabled = false;
    }

    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {
        gameplayParent.DOMove((e.currPlanetTransform.position), e.shiftSpeed).OnComplete(HarvesterAppearence).SetId(10);
    }

    private void HarvesterAppearence()
    {
        harvesterMainMenuParent.position = new(gameplayParent.position.x, 6.0f, 0.0f);
        var comp = harvesterPrefab.GetComponent<HarvesterVisuals>();
        comp.UpdateAvailabilityVisual();
        harvesterMainMenuParent.transform.DOMoveY(0.0f, 0.5f);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            harvestersGroup.SetParent(harvesterMainMenuParent);//

            harvestersGroup.transform.localPosition = Vector3.zero;
            harvestersGroup.transform.rotation = Quaternion.identity;
            harvestersGroup.transform.localScale = Vector3.one;

            harvesterPrefab.localPosition = PREFAB_DEFAULT_POS;
            harvesterPrefab.localEulerAngles = Vector3.zero;
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            harvestersGroup.SetParent(gameplayParent);//

            harvestersGroup.transform.localPosition = Vector3.zero;
            harvestersGroup.transform.rotation = Quaternion.identity;
            harvestersGroup.transform.localScale = Vector3.one;

            harvesterPrefab.localPosition = PREFAB_DEFAULT_POS;
            harvesterPrefab.localEulerAngles = Vector3.zero;
        }
    }

    public void ReturnToInitialPosition()
    {
        gameplayParent.DOMove((Planets.Instance.GetCurrentPlanetPosition()), 1.0f).SetId(10);     
    }
}