using UnityEngine;

public class ShieldEnhanceControl : MonoBehaviour
{
    public static ShieldEnhanceControl Instance { get; private set; }

    [SerializeField] private GameObject gameOverCollider;
    [SerializeField] private GameObject shieldVisuals;
    [SerializeField] private float durationSec;

    private bool startCountdown;
    private float timeCountdown;

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
        ResetEnhance();
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
            if (timeCountdown <= 0)
            {
                DeactivateShield();
                startCountdown = false;
            }
        }
    }

    private void ResetEnhance()
    {
        startCountdown = false;
        timeCountdown = durationSec;
        gameOverCollider.SetActive(false);
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

    public void StartCountdown()
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
