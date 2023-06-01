using System;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleControl : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 10f;  // Rotation speed in degrees per second
    [SerializeField] private Transform vehicleRef;
    [SerializeField] Vector3 halfExtends;
    private Vector2 inputDirection;
    private Vector3 vehicleAngles;
    [SerializeField] private FloatingJoystick joystick;

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }
    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            //Debug.Log("Input" + joystick.Horizontal);
            inputDirection.y = joystick.Horizontal;
            inputDirection.x = joystick.Vertical;
            inputDirection.Normalize();
            MoveVehicle();
        }
    }

    private void MoveVehicle()
    {
        Quaternion rotation = transform.rotation;

        // Rotate around y-axis with left/right arrow keys
        Quaternion horizontalRotation = Quaternion.Euler(0f, 0f, -(inputDirection.y * moveSpeed * Time.deltaTime));
        rotation *= horizontalRotation;

        // Rotate around x-axis with up/down arrow keys
        Quaternion verticalRotation = Quaternion.Euler(inputDirection.x * moveSpeed * Time.deltaTime, 0f, 0f);
        rotation *= verticalRotation;

        // Apply the final rotation
        transform.rotation = rotation;

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
            joystick.CancelInvoke();
        }

        if (GameManager.Instance.IsGamePlaying())
        {

        }
    }
}