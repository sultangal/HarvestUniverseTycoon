using DG.Tweening;
using UnityEngine;

public class HarvesterMovementControl : MonoBehaviour
{
    public static HarvesterMovementControl Instance { get; private set; }

    [SerializeField] private Transform harvesterPrefab;
    [SerializeField] private Transform rotationTable;
    [SerializeField] private Transform harvesterBodyRef;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private float speedEnhanceDurationSec;

    private Vector2 inputDirection;
    private Vector3 vehicleAngles;   
    private float harvesterSpeed;  // Rotation speed in degrees per second
    private readonly float HARVESTER_MIN_SPEED_CONST = 32f;
    private readonly float speedMultNormal = 1f;
    private readonly float speedMultEnhanced = 2f;  
    private readonly float timeZeroToMax = 0.5f;
    private readonly float timeMaxToZero = 0.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0.0f;
    private readonly float wiggleFrequency = 5.0f;
    private readonly float wiggleAmount = 10.0f;
    private readonly float Y_HARVESTER_HEIGHT = 5.381f;
    private bool startSpeedCountdown;
    private float timeSpeedCountdown;

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
        ResetSpeed();
        SetHarvesterGroupStartPosition();
        SetHarvesterSpeed(speedMultNormal);       
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
            GameManager.Instance.GlobalData_.level * Planets.Instance.SPACE_BETWEEN_PLANETS,
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
        if (!GameManager.Instance.IsGamePlaying()) return;
        if (startSpeedCountdown)
        {
            timeSpeedCountdown -= Time.deltaTime;
            if (timeSpeedCountdown <= 0)
            {
                startSpeedCountdown = false;
                SetHarvesterSpeed(speedMultNormal);
            }
        }

        MoveVehicle();

    }

    private void SetHarvesterSpeed(float speed)
    {
        harvesterSpeed = HARVESTER_MIN_SPEED_CONST * speed;
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
           
            ResetSpeed();
            transform.localEulerAngles = Vector3.zero;
            harvesterPrefab.transform.position = new(transform.position.x, 5.381f, 0.726f);
            harvesterPrefab.transform.localEulerAngles = Vector3.zero;
            harvesterBodyRef.transform.localEulerAngles = Vector3.zero;
            harvesterPrefab.SetParent(rotationTable);
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            harvesterPrefab.transform.localEulerAngles = Vector3.zero;
            harvesterPrefab.transform.position = new(transform.position.x, 5.381f, 0.726f);
            harvesterPrefab.SetParent(transform);
        }
    }

    private void Wiggle()
    {
        float wiggle = Mathf.PerlinNoise1D(Time.time * wiggleFrequency);
        harvesterBodyRef.localRotation = Quaternion.Euler(0.0f,
            harvesterBodyRef.localRotation.y,
            wiggle * wiggleAmount);
    }

    private void ResetSpeed()
    {
        startSpeedCountdown = false;
        timeSpeedCountdown = speedEnhanceDurationSec;
        SetHarvesterSpeed(speedMultNormal);
    }

    public bool TryEnhanceSpeed()
    {
        if (GameManager.Instance.TryWithdrawSpeedCost())
        {
            SetHarvesterSpeed(speedMultEnhanced);
            return true;
        } else
            return false;
    }

    public void StartSpeedCountdown()
    {
        startSpeedCountdown = true;
    }

}