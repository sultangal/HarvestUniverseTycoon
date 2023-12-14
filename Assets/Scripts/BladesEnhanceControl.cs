using System;
using UnityEngine;

public class BladesEnhanceControl : MonoBehaviour
{
    public static BladesEnhanceControl Instance { get; private set; }

    [SerializeField] private float durationSec;

    private Transform prefab;
    private Transform bladesGroup;
    private ParticleSystem partSystem;
    private readonly float BLADES_MIN_WIDTH_CONST = 0.5f;
    private readonly float BLADES_COLLIDER_MIN_WIDTH_CONST = 0.075f;

    private readonly float bladesWidthNormal = 1.5f;
    private readonly float bladesWidthEnhanced = 2f;
    private readonly float PARTICLE_MIN_EMITTER_WIDTH = 8f;

    private bool startCountdown;
    private float timeCountdown;

    public Action<float, float, bool> callbackVisuals;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one BladesEnhanceControl!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Store.Instance.OnUpdateHarvesterPrefab += StoreManager_OnUpdateHarvesterPrefab;
        prefab = Store.Instance.GetCurrentPrefab();
        bladesGroup = prefab.GetComponent<HarvesterPrefabRefs>().BladesGroup;
        partSystem = prefab.GetComponent<HarvesterPrefabRefs>().PartSystem;
        ResetBlades();
    }

    private void StoreManager_OnUpdateHarvesterPrefab(object sender, Store.OnUpdateHarvesterPrefabArgs e)
    {
        prefab = e.prefab;
        bladesGroup = prefab.GetComponent<HarvesterPrefabRefs>().BladesGroup;
        partSystem = prefab.GetComponent<HarvesterPrefabRefs>().PartSystem;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            ResetBlades();
        }
    }

    private void SetHarvesterBladesWidth(float bladesWidth)
    {
        BoxCollider bladesCollider = prefab.GetComponent<BoxCollider>();
        bladesCollider.size = new(
            BLADES_COLLIDER_MIN_WIDTH_CONST * bladesWidth,
            bladesCollider.size.y,
            bladesCollider.size.z);
        bladesGroup.transform.localScale = new(
            BLADES_MIN_WIDTH_CONST * bladesWidth,
            bladesGroup.transform.localScale.y,
            bladesGroup.transform.localScale.z);
        var shape = partSystem.shape;
        shape.scale = new(PARTICLE_MIN_EMITTER_WIDTH * bladesWidth,
            partSystem.shape.scale.y,
            partSystem.shape.scale.z);
    }

    private void ResetAllHarvestersBlades()
    {

    }

    private void ResetBlades()
    {
        startCountdown = false;
        timeCountdown = durationSec;
        SetHarvesterBladesWidth(bladesWidthNormal);
    }

    public bool TryEnhanceBlades()
    {
        if (GameManager.Instance.TryWithdrawBladesCost())
        {
            SetHarvesterBladesWidth(bladesWidthEnhanced);
            return true;
        }
        return false;
    }

    public void StartBladesCountdown()
    {
        startCountdown = true;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (startCountdown)
        {
            timeCountdown -= Time.deltaTime;
            callbackVisuals(Time.deltaTime, durationSec, true);
            if (timeCountdown <= 0)
            {
                startCountdown = false;
                SetHarvesterBladesWidth(bladesWidthNormal);
                callbackVisuals(Time.deltaTime, durationSec, false);
            }           
        }
    }
}
