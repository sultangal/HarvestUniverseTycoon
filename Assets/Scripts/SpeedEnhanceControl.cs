using System;
using UnityEngine;

public class SpeedEnhanceControl : MonoBehaviour
{
    [SerializeField] private float speedEnhanceDurationSec;

    private readonly float HARVESTER_MIN_SPEED_CONST = 32f;
    private readonly float speedMultNormal = 1f;
    private readonly float speedMultEnhanced = 2f;
    private bool startCountdown;
    private float timeSpeedCountdown;

    public Action<float, float, bool> callbackVisuals;

    public static SpeedEnhanceControl Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one HarvesterMenuPosition!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        ResetSpeed();
        SetHarvesterSpeed(speedMultNormal);
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (startCountdown)
        {
            timeSpeedCountdown -= Time.deltaTime;
            callbackVisuals(Time.deltaTime, speedEnhanceDurationSec, true);
            if (timeSpeedCountdown <= 0)
            {
                startCountdown = false;
                SetHarvesterSpeed(speedMultNormal);
                callbackVisuals(Time.deltaTime, speedEnhanceDurationSec, false);
            }
        }
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            ResetSpeed();
        }
    }

    private void SetHarvesterSpeed(float speed)
    {
        HarvesterMovementControl.Instance.harvesterSpeed = HARVESTER_MIN_SPEED_CONST * speed;
    }

    private void ResetSpeed()
    {
        startCountdown = false;
        timeSpeedCountdown = speedEnhanceDurationSec;
        SetHarvesterSpeed(speedMultNormal);
    }

    public bool TryEnhanceSpeed()
    {
        if (GameManager.Instance.TryWithdrawSpeedCost())
        {
            SetHarvesterSpeed(speedMultEnhanced);
            return true;
        }
        else
            return false;
    }

    public void StartCountdown()
    {
        startCountdown = true;
    }


}
