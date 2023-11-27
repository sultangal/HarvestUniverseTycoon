using DG.Tweening;
using UnityEngine;

public class MenuControl : MonoBehaviour
{
    public static MenuControl Instance { get; private set; }

    [SerializeField] private Transform harvesterPrefab;
    [SerializeField] private Transform harvesterMainMenuParent;
    [SerializeField] private Transform gameplayParent;

    private readonly Vector3 PREFAB_DEFAULT_POS = new(0.0f, 5.381f, 0.726f);

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
        HarvesterAppearence();
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
            harvesterPrefab.SetParent(harvesterMainMenuParent);
            harvesterPrefab.localPosition = PREFAB_DEFAULT_POS;
            harvesterPrefab.localEulerAngles = Vector3.zero;
        }

        if (GameManager.Instance.IsGamePlaying())
        { 
            harvesterPrefab.localEulerAngles = Vector3.zero;
            harvesterPrefab.localPosition = PREFAB_DEFAULT_POS;
            harvesterPrefab.SetParent(gameplayParent);
        }
    }

    public void ReturnToInitialPosition()
    {
        gameplayParent.DOMove((Planets.Instance.GetCurrentPlanetPosition()), 1.0f).SetId(10);     
    }
}