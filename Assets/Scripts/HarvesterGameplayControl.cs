using UnityEngine;

public class HarvesterGameplayControl : MonoBehaviour
{
    public static HarvesterGameplayControl Instance { get; private set; }

    [SerializeField] private Transform prefab;
    [SerializeField] private Transform bodyGroup;
    [SerializeField] private FloatingJoystick joystick;

    public float speed;  // Rotation speed in degrees per second 
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0.0f;
    private readonly float wiggleFrequency = 5.0f;
    private readonly float wiggleAmount = 10.0f;

    private Vector2 inputDirection;
    private Vector3 vehicleAngles;

    private readonly float timeZeroToMax = 0.5f;
    private readonly float timeMaxToZero = 0.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one HarvesterGameplayControl!");
            return;
        }
        Instance = this;
        SetStartPosition();
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        accelRatePerSec = speed / timeZeroToMax;
        decelRatePerSec = -speed / timeMaxToZero; 
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
            MoveVehicle();
    }

    private void SetStartPosition()
    {
        transform.position = new(
            GameManager.Instance.GlobalData_.level * Planets.Instance.SPACE_BETWEEN_PLANETS,
            0f,
            0f);
    }

    private void MoveVehicle()
    {
        if (joystick.Horizontal != 0.0f && joystick.Vertical != 0.0f)
        {
            Wiggle();
            //calculation of acceleration
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Min(forwardVelocity, speed);

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
            vehicleAngles.x = prefab.transform.localEulerAngles.x;
            vehicleAngles.y = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            vehicleAngles.z = prefab.transform.localEulerAngles.z;
            prefab.transform.localEulerAngles = vehicleAngles;
        }
    }

    private void Wiggle()
    {
        float wiggle = Mathf.PerlinNoise1D(Time.time * wiggleFrequency);
        bodyGroup.localRotation = Quaternion.Euler(0.0f,
            bodyGroup.localRotation.y,
            wiggle * wiggleAmount);
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
            bodyGroup.transform.localEulerAngles = Vector3.zero;
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            transform.position = Planets.Instance.GetCurrentPlanetPosition();
        }
    }

}
