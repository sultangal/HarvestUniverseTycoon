using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationMultiplier = 500;
    [SerializeField] private bool xAxis;
    [SerializeField] private bool yAxis;
    [SerializeField] private bool zAxis;
    private Vector3 rotation = new(0.0f, 0.0f, 0.0f);
    // Update is called once per frame
    private void Update()
    {
        if (xAxis)
            rotation.x = Time.deltaTime * rotationMultiplier;
        if (yAxis)
            rotation.y = Time.deltaTime * rotationMultiplier;
        if (zAxis)
            rotation.z = Time.deltaTime * rotationMultiplier;
        transform.Rotate(rotation);
    }
}
