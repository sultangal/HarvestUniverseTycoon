using DG.Tweening;
using UnityEngine;

public class HarvesterGroup : MonoBehaviour
{
    public static HarvesterGroup Instance { get; private set; }

    [SerializeField] private Transform harvesterPrefab;
    [SerializeField] private Transform rotationTable;
    [SerializeField] private Transform harvesterBodyRef;
    [SerializeField] private Transform harvesterBladesGroupRef;   
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private ParticleSystem partSystem;

    private Vector2 inputDirection;
    private Vector3 vehicleAngles;   
    private float harvesterSpeed;  // Rotation speed in degrees per second
    private readonly float HARVESTER_MIN_SPEED_CONST = 32f;
    public float harvesterSpeedMult = 1f;
    private readonly float BLADES_MIN_WIDTH_CONST = 0.5f;
    private readonly float BLADES_COLLIDER_MIN_WIDTH_CONST = 0.075f;
    private readonly float bladesWidthNormal = 1.5f;
    private readonly float bladesWidthEnhanced = 2f;
    private readonly float PARTICLE_MIN_EMITTER_WIDTH = 8f;
    private readonly float timeZeroToMax = 0.5f;
    private readonly float timeMaxToZero = 0.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0.0f;

    private readonly float wiggleFrequency = 5.0f;
    private readonly float wiggleAmount = 10.0f;

    private readonly float Y_HARVESTER_HEIGHT = 5.381f;

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
        SetHarvesterGroupStartPosition();
        SetHarvesterSpeed();
        SetHarvesterBladesWidth(bladesWidthNormal);
        harvesterPrefab.SetParent(rotationTable);
        accelRatePerSec = harvesterSpeed / timeZeroToMax;
        decelRatePerSec = -harvesterSpeed / timeMaxToZero;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Planets.Instance.OnPlanetShift += PlanetsController_OnPlanetShift;
        HarvesterAppearence();
    }

    private void SetHarvesterGroupStartPosition()
    {
        transform.position = new(
            GameManager.Instance.GlobalData_.level*Planets.Instance.SPACE_BETWEEN_PLANETS,
            0f,
            0f);
    }

    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {
        transform.DOMove((e.currPlanetTransform.position), e.shiftSpeed).OnComplete(HarvesterAppearence);
    }

    private void HarvesterAppearence()
    {
        rotationTable.transform.position = new(transform.position.x, Y_HARVESTER_HEIGHT, 0.0f);
        var comp = harvesterPrefab.GetComponent<HarvesterVisuals>();
        comp.UpdateAvailabilityVisual();
        rotationTable.transform.DOMoveY(0.0f, 0.5f);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            MoveVehicle();
        }
    }

    private void SetHarvesterSpeed()
    {
        harvesterSpeed = HARVESTER_MIN_SPEED_CONST * harvesterSpeedMult;
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

    private void MoveVehicle()
    {

        if (joystick.Horizontal != 0.0f && joystick.Vertical != 0.0f)
        {
            Wiggle();
            //calculation of acceleration
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Min(forwardVelocity, harvesterSpeed);

            //saving last entered direction
            inputDirection.y = joystick.Horizontal;
            inputDirection.x = joystick.Vertical;
            //normalizing to avoid when vehicle move faster in diagonal directions
            inputDirection.Normalize();
        }
        else
        {
            //calculation of deceleration
            forwardVelocity += decelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Max(forwardVelocity, 0);
        }

        transform.rotation *= Quaternion.Euler(
            inputDirection.x * forwardVelocity * Time.deltaTime,
            0f,
            -(inputDirection.y * forwardVelocity * Time.deltaTime));
        Direction();       
    }

    private void Direction()
    {
        if (inputDirection != Vector2.zero)
        {
            vehicleAngles.x = harvesterPrefab.transform.localEulerAngles.x;
            vehicleAngles.y = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            vehicleAngles.z = harvesterPrefab.transform.localEulerAngles.z;

            harvesterPrefab.transform.localEulerAngles = vehicleAngles;
        }
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsTimeIsUp() || GameManager.Instance.IsGameOver())
        {
            inputDirection.y = 0.0f;
            inputDirection.x = 0.0f;
            //Reset joystick position
            joystick.OnPointerUp(null);
        }

        if (GameManager.Instance.IsGameWaitingToStart())
        {            
            transform.localEulerAngles = Vector3.zero;
            harvesterPrefab.transform.position = new(transform.position.x, 5.381f, 0.726f);
            harvesterPrefab.transform.localEulerAngles = Vector3.zero;
            harvesterBodyRef.transform.localEulerAngles = Vector3.zero;
            harvesterPrefab.SetParent(rotationTable);
            SetHarvesterBladesWidth(bladesWidthNormal);
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            harvesterPrefab.transform.localEulerAngles = Vector3.zero;
            harvesterPrefab.transform.position = new(transform.position.x, 5.381f, 0.726f);
            harvesterPrefab.SetParent(transform);
            SetHarvesterSpeed();
            
        }
    }

    private void Wiggle()
    {
        float wiggle = Mathf.PerlinNoise1D(Time.time * wiggleFrequency);
        harvesterBodyRef.localRotation = Quaternion.Euler(0.0f,
            harvesterBodyRef.localRotation.y,
            wiggle * wiggleAmount);
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


}