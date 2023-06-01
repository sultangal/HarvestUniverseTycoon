using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FieldController : MonoBehaviour
{
    [SerializeField] private FieldItemSO fieldItemSO;
    [SerializeField] private Transform meshForPointsSource;

    private List<Transform> items;

    private readonly System.Random random = new();
    private float randomMultiplier = 0.2f;
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
            //Vector3 position = vertex;
            Vector3 position = new((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            position *= randomMultiplier;
            position += vertex;
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));


            Transform item = Instantiate(fieldItemSO.itemPrefab, position, Quaternion.LookRotation(position));
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
