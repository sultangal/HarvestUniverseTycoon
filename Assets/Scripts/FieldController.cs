using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{
    [SerializeField] private FieldItemSO fieldItemSO;
    [SerializeField] private Transform meshForPointsSource;

    private List<Transform> items;

    private readonly System.Random random = new();
    private float randomMultiplier = 1f;
    void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        items = new();
        InstantiateFieldItems();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameSessionEnded())
        {
            DestroyFieldItems();
            InstantiateFieldItems();
        }
    }

    private void InstantiateFieldItems()
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
            item.Rotate(new(0.0f, (float)random.NextDouble() * 100, 0.0f));
            items.Add(item);
        }
    }

    private void DestroyFieldItems()
    {
        foreach (var item in items)
        {
            if (item == null) continue; 
            Destroy(item.gameObject);
        }
        items.Clear();
    }   
}
