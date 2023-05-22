using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private FieldItemSO fieldItemSO;
    [SerializeField] private Transform meshForPointsSource;
    

    private System.Random random = new System.Random();
    private float randomMultiplier = 1f;
    void Start()
    {
        Mesh mesh = meshForPointsSource.GetComponent<MeshFilter>().mesh;
        foreach (var vertex in mesh.vertices)
        {
            // Instantiate(fieldUnit, vertex, Quaternion.LookRotation(vertex));
            Vector3 rndBias = new((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            rndBias *= randomMultiplier;
            rndBias += vertex;
            rndBias.Normalize();
            rndBias.Scale(new(5f, 5f, 5f));
            Transform item = Instantiate(fieldItemSO.itemPrefab, rndBias, Quaternion.LookRotation(rndBias));
            Vector3 turnPlease = new(90.0f, 0.0f, 0.0f);
            item.eulerAngles += turnPlease;
            item.Rotate(new(0.0f, (float)random.NextDouble()*100, 0.0f));
        }        
    }
}
