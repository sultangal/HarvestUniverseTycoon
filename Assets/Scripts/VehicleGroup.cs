using DG.Tweening;
using UnityEngine;

public class VehicleGroup : MonoBehaviour
{

    [SerializeField] private float maxSpeed = 50f;  // Rotation speed in degrees per second
    [SerializeField] private Transform vehicleRef;
    [SerializeField] private Transform vehicleBodyRef;
    //[SerializeField] Vector3 halfExtends;
    private Vector2 inputDirection;
    private Vector3 vehicleAngles;
    [SerializeField] private FloatingJoystick joystick;

    private readonly float timeZeroToMax = 0.5f;
    private readonly float timeMaxToZero = 0.5f;
    private float accelRatePerSec;
    private float decelRatePerSec;
    private float forwardVelocity = 0.0f;

    private readonly float wiggleFrequency = 5.0f;
    private readonly float wiggleAmount = 10.0f;

   

    private void Start()
    {
        accelRatePerSec = maxSpeed / timeZeroToMax;
        decelRatePerSec = -maxSpeed / timeMaxToZero;
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

    private void MoveVehicle()
    {

        if (joystick.Horizontal != 0.0f && joystick.Vertical != 0.0f)
        {
            Wiggle();
            //calculation of acceleration
            forwardVelocity += accelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Min(forwardVelocity, maxSpeed);

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
            vehicleAngles.x = vehicleRef.transform.localEulerAngles.x;
            vehicleAngles.y = Mathf.Atan2(inputDirection.y, inputDirection.x) * Mathf.Rad2Deg;
            vehicleAngles.z = vehicleRef.transform.localEulerAngles.z;

            vehicleRef.transform.localEulerAngles = vehicleAngles;
        }
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameSessionEnded())
        {
            inputDirection.y = 0.0f;
            inputDirection.x = 0.0f;
            //Reset joystick position
            joystick.OnPointerUp(null);
        }

        if (GameManager.Instance.IsGamePlaying())
        {
            transform.localEulerAngles = Vector3.zero;
            vehicleRef.transform.localEulerAngles = Vector3.zero;
        }
    }

    private void Wiggle()
    {
        float wiggle = Mathf.PerlinNoise1D(Time.time * wiggleFrequency);
        vehicleBodyRef.localRotation = Quaternion.Euler(0.0f,
            vehicleBodyRef.localRotation.y,
            wiggle * wiggleAmount);
    }
}