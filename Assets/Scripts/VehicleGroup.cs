using DG.Tweening;
using UnityEngine;

public class VehicleGroup : MonoBehaviour
{
    [Header("Settings")]
    [Range(1.0f, 1.5f)]
    [SerializeField] private float speedMult = 1f;    
    [Range(1.0f, 1.5f)]
    [SerializeField] private float bladesWidthMult = 1f;

    [Header("References")]
    [SerializeField] private Transform harvesterRef;
    [SerializeField] private Transform harvesterBodyRef;
    [SerializeField] private Transform harvesterBladesGroupRef;
   
    [SerializeField] private FloatingJoystick joystick;
    
    private Vector2 inputDirection;
    private Vector3 vehicleAngles;
    private readonly float HARVESTER_SPEED_CONST = 32f;
    private float harvesterSpeed;  // Rotation speed in degrees per second
    private readonly float timeZeroToMax = 0.5f;
    private readonly float timeMaxToZero = 0.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0.0f;

    private readonly float wiggleFrequency = 5.0f;
    private readonly float wiggleAmount = 10.0f;  

    private void Start()
    {
        SetHarvesterSettings();
        accelRatePerSec = harvesterSpeed / timeZeroToMax;
        decelRatePerSec = -harvesterSpeed / timeMaxToZero;
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Planets.Instance.OnPlanetShift += PlanetsController_OnPlanetShift;
    }

    private void PlanetsController_OnPlanetShift(object sender, Planets.OnPlanetShiftEventArgs e)
    {
        transform.DOMove((e.currPlanetTransform.position), 1);
    }

    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            MoveVehicle();
        }
    }

    private void SetHarvesterSettings()
    {
        harvesterSpeed = HARVESTER_SPEED_CONST * speedMult;
        BoxCollider vehicleBoxCollider = harvesterRef.GetComponent<BoxCollider>();
        vehicleBoxCollider.size = new(
            vehicleBoxCollider.size.x * bladesWidthMult, 
            vehicleBoxCollider.size.y, 
            vehicleBoxCollider.size.z);
        harvesterBladesGroupRef.transform.localScale = new(
            harvesterBladesGroupRef.transform.localScale.x * bladesWidthMult,
            harvesterBladesGroupRef.transform.localScale.y,
            harvesterBladesGroupRef.transform.localScale.z);
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
            vehicleAngles.x = harvesterRef.transform.localEulerAngles.x;
            vehicleAngles.y = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            vehicleAngles.z = harvesterRef.transform.localEulerAngles.z;

            harvesterRef.transform.localEulerAngles = vehicleAngles;
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
            harvesterRef.transform.localEulerAngles = Vector3.zero;
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            SetHarvesterSettings();
        }
    }

    private void Wiggle()
    {
        float wiggle = Mathf.PerlinNoise1D(Time.time * wiggleFrequency);
        harvesterBodyRef.localRotation = Quaternion.Euler(0.0f,
            harvesterBodyRef.localRotation.y,
            wiggle * wiggleAmount);
    }
}