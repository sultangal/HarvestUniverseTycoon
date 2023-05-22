using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private float rotationMultiplier = 500;
    private Vector3 rotation = new(0.0f, 0.0f, 0.0f);
    // Update is called once per frame
    private void Update()
    {
        rotation.x = Time.deltaTime * rotationMultiplier;
        transform.Rotate(rotation);
    }
}
