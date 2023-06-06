using UnityEngine;

public class Bend : MonoBehaviour
{

    public bool isBend = false;
    [SerializeField] private float bendAngle = 50f;
    private Vector3 initialRotation = Vector3.zero;
    private Vector3 bendedRotation = Vector3.zero;

    private void Start()
    {
        initialRotation = transform.localEulerAngles;
        bendedRotation = new(transform.localEulerAngles.x + bendAngle, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }

    private void Update()
    {
        transform.localEulerAngles = isBend ? bendedRotation : initialRotation;
    }


}
