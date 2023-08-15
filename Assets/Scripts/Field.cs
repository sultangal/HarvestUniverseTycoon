using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Mesh meshForPointsSource;

    public List<Transform> Items { get; private set; } = new();

    private readonly System.Random random = new();
    private readonly float randomMultiplier = 0.02f;
    private Planets planets;
    private void Start()
    {
        if (!TryGetComponent(out planets))
        {
            Debug.LogError("Planets script not founded. In order to work properly, gameObject has to reference Planets script.");
            return;
        }

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            DestroyFieldItems();
            InstantiateFieldItems(planets.GetCurrentPlanetSO().fieldItemSO[0].fieldItemPrefab,
                planets.GetCurrentPlanetSO().fieldItemSO[1].fieldItemPrefab,
                planets.GetCurrentPlanetSO().planetPrefab.position);
        }

        if (GameManager.Instance.IsGameWaitingToStart())
        {
            DestroyFieldItems();
        }
    }

    public void InstantiateFieldItems(Transform itemRef1, Transform itemRef2, Vector3 planetPosition)
    {
        for (int i = 0; i < meshForPointsSource.vertices.Length; i++)
        {
            Vector3 position = new((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            position *= randomMultiplier;
            position += meshForPointsSource.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color
            Transform item;
            if (!meshForPointsSource.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source dont have vertex color attribute");
                return;
            } else
            {
                if (meshForPointsSource.colors[i].r > 0.5f)
                    item = Instantiate(itemRef1, position, Quaternion.LookRotation(position));
                else
                    item = Instantiate(itemRef2, position, Quaternion.LookRotation(position));
            }

            Vector3 turnItem = new(90.0f, 0.0f, 0.0f);
            item.eulerAngles += turnItem;
            item.Rotate(new(0.0f, (float)random.NextDouble() * 360f, 0.0f));
            item.position += planetPosition;
            Items.Add(item);
        }
    }

    public void Shift(Vector3 position)
    {
        Items.ForEach(e => e.gameObject.transform.position += position);
    }

    public void OffVisibility()
    {
        Items.ForEach(e => e.gameObject.SetActive(!gameObject.activeSelf));
    }

    public void OnVisibility()
    {
        Items.ForEach(e => e.gameObject.SetActive(gameObject.activeSelf));
    }

    private void DestroyFieldItems()
    {
        foreach (var item in Items)
        {
            if (item == null) continue;
            Destroy(item.gameObject);
        }
        Items.Clear();
    }


}

