using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuControl : MonoBehaviour
{
    public static MenuControl Instance { get; private set; }

    [SerializeField] private Transform harvesterGroup;
    [SerializeField] private Transform mainMenuParent;
    [SerializeField] private Transform gameplayParent;

    private Transform harvesterPrefab;
    private static readonly Vector3 PREFAB_START_POS = new(0.0f, 5.381f, 0.726f);

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
        StoreManager.Instance.OnStoreEnter += MainMenuUI_OnStoreEnter;
        StoreManager.Instance.OnBackToMainMenu += MainMenuUI_OnBackToMainMenu;
        StoreManager.Instance.OnUpdateHarvesterPrefab += StoreManager_OnUpdateHarvesterPrefab;

        harvesterPrefab = StoreManager.Instance.GetCurrentPrefab();
        harvesterGroup.SetParent(mainMenuParent);
        HarvesterAppearence();
    }

    private void StoreManager_OnUpdateHarvesterPrefab(object sender , StoreManager.OnUpdateHarvesterPrefabArgs e)
    {
        harvesterPrefab = e.prefab;
    }

    private void MainMenuUI_OnBackToMainMenu(object sender, System.EventArgs e)
    {
        harvesterPrefab.GetComponent<Rotation>().enabled = true;
    }

    private void MainMenuUI_OnStoreEnter(object sender, System.EventArgs e)
    {
        harvesterPrefab.GetComponent<Rotation>().enabled = false;
    }

    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {
        gameplayParent.DOMove((e.currPlanetTransform.position), e.shiftSpeed).OnComplete(HarvesterAppearence).SetId(10);
    }

    private void HarvesterAppearence()
    {
        mainMenuParent.position = new(gameplayParent.position.x, 6.0f, 0.0f);
        var comp = harvesterPrefab.GetComponent<HarvesterVisuals>();
        comp.UpdateAvailabilityVisual();
        mainMenuParent.transform.DOMoveY(0.0f, 0.5f);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            harvesterGroup.SetParent(mainMenuParent);
            harvesterPrefab.GetComponent<Rotation>().enabled = true;
            harvesterPrefab.GetComponent<HarvesterVisuals>().SetPivotToMenuMode();

            var posX = StoreManager.Instance.EvaluateCurrAvailablePrefabPosX();

            ResetHarvesterGroupTransform(new(-posX, 0f, 0f));
            ResetHarvesterPrefabTransform(new(posX, PREFAB_START_POS.y, PREFAB_START_POS.z));
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            harvesterGroup.SetParent(gameplayParent);
            harvesterPrefab.GetComponent<Rotation>().enabled = false;
            harvesterPrefab.GetComponent<HarvesterVisuals>().SetPivotToGameplayMode();

            ResetHarvesterGroupTransform(Vector3.zero);
            ResetHarvesterPrefabTransform(PREFAB_START_POS);
        }
    }

    private void ResetHarvesterPrefabTransform(Vector3 position)
    {
        harvesterPrefab.localPosition = position;
        harvesterPrefab.localEulerAngles = Vector3.zero;
    }

    private void ResetHarvesterGroupTransform(Vector3 position)
    {
        harvesterGroup.transform.localPosition = position;
        harvesterGroup.transform.rotation = Quaternion.identity;
        harvesterGroup.transform.localScale = Vector3.one;
    }

    public void ReturnToInitialPosition()
    {
        gameplayParent.DOMove((Planets.Instance.GetCurrentPlanetPosition()), 1.0f).SetId(10);     
    }
}