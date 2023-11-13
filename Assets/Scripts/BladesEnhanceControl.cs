using UnityEngine;

public class BladesEnhanceControl : MonoBehaviour
{
    public static BladesEnhanceControl Instance { get; private set; }

    [SerializeField] private Transform harvesterPrefab;
    [SerializeField] private Transform harvesterBladesGroupRef;
    [SerializeField] private ParticleSystem partSystem;
    [SerializeField] private float enhanceBladesTimeSec;

    private readonly float BLADES_MIN_WIDTH_CONST = 0.5f;
    private readonly float BLADES_COLLIDER_MIN_WIDTH_CONST = 0.075f;

    private readonly float bladesWidthNormal = 1.5f;
    private readonly float bladesWidthEnhanced = 2f;
    private readonly float PARTICLE_MIN_EMITTER_WIDTH = 8f;

    private bool startBladesCountdown;
    private float timeBladesCountdown;

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
        ResetBlades();
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
        BoxCollider bladesCollider = harvesterPrefab.GetComponent<BoxCollider>();
        bladesCollider.size = new(
            BLADES_COLLIDER_MIN_WIDTH_CONST * bladesWidth,
            bladesCollider.size.y,
            bladesCollider.size.z);
        harvesterBladesGroupRef.transform.localScale = new(
            BLADES_MIN_WIDTH_CONST * bladesWidth,
            harvesterBladesGroupRef.transform.localScale.y,
            harvesterBladesGroupRef.transform.localScale.z);
        var shape = partSystem.shape;
        shape.scale = new(PARTICLE_MIN_EMITTER_WIDTH * bladesWidth,
            partSystem.shape.scale.y,
            partSystem.shape.scale.z);
    }

    private void ResetBlades()
    {
        startBladesCountdown = false;
        timeBladesCountdown = enhanceBladesTimeSec;
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
        startBladesCountdown = true;
    }

    private void Update()
    {
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (startBladesCountdown)
        {
            timeBladesCountdown -= Time.deltaTime;
            if (timeBladesCountdown <= 0)
            {
                startBladesCountdown = false;
                SetHarvesterBladesWidth(bladesWidthNormal);
            }
        }
    }
}
