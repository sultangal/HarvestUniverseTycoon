using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpherePointsGenerator : MonoBehaviour
{
    private System.Random random = new System.Random();

    public Vector3 GenerateRandomPointOnSphere(float radius)
    {
        float u = (float)random.NextDouble();
        float v = (float)random.NextDouble();

        float theta = 2 * Mathf.PI * u;
        float phi = Mathf.Acos(2 * v - 1);

        float x = radius * Mathf.Sin(phi) * Mathf.Cos(theta);
        float y = radius * Mathf.Sin(phi) * Mathf.Sin(theta);
        float z = radius * Mathf.Cos(phi);

        return new Vector3(x, y, z);
    }

    private void Start()
    {
        float radius = 1.0f; // Radius of the sphere

        // Generate 10 random points on the sphere
        for (int i = 0; i < 2000; i++)
        {
            Vector3 point = GenerateRandomPointOnSphere(radius);
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localScale = Vector3.one/50;
            cube.transform.position = point;
            Debug.Log($"Point {i + 1}: {point}");
        }
    }
}