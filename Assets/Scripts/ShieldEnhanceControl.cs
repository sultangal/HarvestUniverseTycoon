using System;
using UnityEngine;

public class ShieldEnhanceControl : MonoBehaviour
{
    public static ShieldEnhanceControl Instance { get; private set; }

    [SerializeField] private float durationSec;

    private GameObject gameOverCollider;
    private GameObject shieldVisuals;
    private bool startCountdown;
    private float timeCountdown;

    public Action<float, float, bool> callbackVisuals;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one ShieldEnhanceControl!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        StoreManager.Instance.OnUpdateHarvesterPrefab += HarvestersStore_OnUpdateHarvesterPrefab;
        var prefab = StoreManager.Instance.GetCurrentPrefab();
        gameOverCollider = prefab.GetComponent<HarvesterPrefabRefs>().GameOverCollider;
        shieldVisuals = prefab.GetComponent<HarvesterPrefabRefs>().ShiledVisuals;
        ResetEnhance();
    }

    private void HarvestersStore_OnUpdateHarvesterPrefab(object sender, StoreManager.OnUpdateHarvesterPrefabArgs e)
    {
        var prefab = e.prefab;
        gameOverCollider = prefab.GetComponent<HarvesterPrefabRefs>().GameOverCollider;
        shieldVisuals = prefab.GetComponent<HarvesterPrefabRefs>().ShiledVisuals;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            ResetEnhance();
        }
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
                DeactivateShield();
                startCountdown = false;
                callbackVisuals(Time.deltaTime, durationSec, false);
            }
        }
    }

    private void ResetEnhance()
    {
        startCountdown = false;
        timeCountdown = durationSec;
        gameOverCollider.SetActive(true);
        shieldVisuals.SetActive(false);
    }

    public bool TryShiledEnhance()
    {
        if (GameManager.Instance.TryWithdrawShieldCost())
        {
            ActivateShield();
            return true;
        }
        return false;
    }

    public void StartShieldCountdown()
    {
        startCountdown = true;
    }

    public void ActivateShield()
    {
        gameOverCollider.SetActive(false);
        shieldVisuals.SetActive(true);
    }

    public void DeactivateShield()
    {
        gameOverCollider.SetActive(true);
        shieldVisuals.SetActive(false);
    }
}
