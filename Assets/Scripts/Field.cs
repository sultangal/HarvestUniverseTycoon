using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Mesh meshForPointsSource_2_Items;
    public Mesh meshForPointsSource_3_Items;
    public Mesh meshForPointsSource_4_Items;
    public Mesh meshForPointsSource_5_Items;
    public Mesh meshForPointsSource_6_Items;
    public Mesh meshForPointsSource_7_Items;
    public Mesh meshForPointsSource_8_Items;

    public List<Transform> Items { get; private set; } = new();
    private readonly float randomMultiplier = 0.02f;
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            //DestroyFieldItems();
            //InstantiateFieldItems();
        }

        if (GameManager.Instance.IsGameWaitingToStart())
        {
            //DestroyFieldItems();
        }
    }

    private void InstantiateFieldItems()
    {
        switch (GameManager.Instance.GameSessionData_.FieldItemsSOonLevel.Length)
        {
            case 2:
                Instantiate_2_Items(GameManager.Instance.GameSessionData_.FieldItemsSOonLevel);
                break;
            case 3:
                Instantiate_3_Items(GameManager.Instance.GameSessionData_.FieldItemsSOonLevel);
                break;
            case 4:
                Instantiate_4_Items(GameManager.Instance.GameSessionData_.FieldItemsSOonLevel);
                break;
            case 5:
                Instantiate_5_Items(GameManager.Instance.GameSessionData_.FieldItemsSOonLevel);
                break;
            case 6:
                Instantiate_6_Items(GameManager.Instance.GameSessionData_.FieldItemsSOonLevel);
                break;
            case 7:
                Instantiate_7_Items(GameManager.Instance.GameSessionData_.FieldItemsSOonLevel);
                break;
            case 8:
                Instantiate_8_Items(GameManager.Instance.GameSessionData_.FieldItemsSOonLevel);
                break;
            default:
                Debug.LogError("FieldItemsSO Array not correct. Check it.");
                break;
        }
    }
    private void Instantiate_2_Items(FieldItemSO[] fieldItemsSOonLevel)
    {
        if (fieldItemsSOonLevel.Length != 2) Debug.LogError("Items count does not match. You've made a mistake");
        for (int i = 0; i < meshForPointsSource_2_Items.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource_2_Items.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color            
            if (!meshForPointsSource_2_Items.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source don't have vertex color attribute");
                return;
            }
            Transform item;
            float vertColor = meshForPointsSource_2_Items.colors[i].r;
            if (vertColor == 0.0f)
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
                item = Instantiate(fieldItemsSOonLevel[1].fieldItemPrefab, position, Quaternion.LookRotation(position));

            PositioningItem(item);
        }
    }
    private void Instantiate_3_Items(FieldItemSO[] fieldItemsSOonLevel)
    {
        if (fieldItemsSOonLevel.Length != 3) Debug.LogError("Items count does not match. You've made a mistake");
        for (int i = 0; i < meshForPointsSource_3_Items.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource_3_Items.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color            
            if (!meshForPointsSource_3_Items.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source don't have vertex color attribute");
                return;
            }
            Transform item;
            float vertColor = meshForPointsSource_3_Items.colors[i].r;
            if (vertColor == 0.0f)
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.33f && vertColor < 0.66f)
                item = Instantiate(fieldItemsSOonLevel[1].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor == 1.0f)
                item = Instantiate(fieldItemsSOonLevel[2].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));

            PositioningItem(item);
        }
    }
    private void Instantiate_4_Items(FieldItemSO[] fieldItemsSOonLevel)
    {
        if (fieldItemsSOonLevel.Length != 4) Debug.LogError("Items count does not match. You've made a mistake");
        for (int i = 0; i < meshForPointsSource_4_Items.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource_4_Items.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color            
            if (!meshForPointsSource_4_Items.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source don't have vertex color attribute");
                return;
            }
            Transform item;
            float vertColor = meshForPointsSource_4_Items.colors[i].r;
            if (vertColor == 0.0f)
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.25f && vertColor < 0.5f)
                item = Instantiate(fieldItemsSOonLevel[1].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.5f && vertColor < 0.75f)
                item = Instantiate(fieldItemsSOonLevel[2].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor == 1.0f)
                item = Instantiate(fieldItemsSOonLevel[3].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));

            PositioningItem(item);
        }
    }
    private void Instantiate_5_Items(FieldItemSO[] fieldItemsSOonLevel)
    {
        if (fieldItemsSOonLevel.Length != 5) Debug.LogError("Items count does not match. You've made a mistake");
        for (int i = 0; i < meshForPointsSource_5_Items.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource_5_Items.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color            
            if (!meshForPointsSource_5_Items.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source don't have vertex color attribute");
                return;
            }
            Transform item;
            float vertColor = meshForPointsSource_5_Items.colors[i].r;
            if (vertColor == 0.0f)
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.2f && vertColor < 0.4f)
                item = Instantiate(fieldItemsSOonLevel[1].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.4f && vertColor < 0.6f)
                item = Instantiate(fieldItemsSOonLevel[2].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.6f && vertColor < 0.8f)
                item = Instantiate(fieldItemsSOonLevel[3].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor == 1.0f)
                item = Instantiate(fieldItemsSOonLevel[4].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));

            PositioningItem(item);
        }
    }
    private void Instantiate_6_Items(FieldItemSO[] fieldItemsSOonLevel)
    {
        if (fieldItemsSOonLevel.Length != 6) Debug.LogError("Items count does not match. You've made a mistake");
        for (int i = 0; i < meshForPointsSource_6_Items.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource_6_Items.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color            
            if (!meshForPointsSource_6_Items.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source don't have vertex color attribute");
                return;
            }
            Transform item;
            float vertColor = meshForPointsSource_6_Items.colors[i].r;
            if (vertColor == 0.0f)
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.167f && vertColor < 0.333f)
                item = Instantiate(fieldItemsSOonLevel[1].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.333f && vertColor < 0.5f)
                item = Instantiate(fieldItemsSOonLevel[2].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.5f && vertColor < 0.666f)
                item = Instantiate(fieldItemsSOonLevel[3].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.666f && vertColor < 0.833f)
                item = Instantiate(fieldItemsSOonLevel[4].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor == 1.0f)
                item = Instantiate(fieldItemsSOonLevel[5].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));

            PositioningItem(item);
        }
    }
    private void Instantiate_7_Items(FieldItemSO[] fieldItemsSOonLevel)
    {
        if (fieldItemsSOonLevel.Length != 7) Debug.LogError("Items count does not match. You've made a mistake");
        for (int i = 0; i < meshForPointsSource_7_Items.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource_7_Items.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color            
            if (!meshForPointsSource_7_Items.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source don't have vertex color attribute");
                return;
            }
            Transform item;
            float vertColor = meshForPointsSource_7_Items.colors[i].r;
            if (vertColor == 0.0f)
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.143f && vertColor < 0.286f)
                item = Instantiate(fieldItemsSOonLevel[1].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.286f && vertColor < 0.428f)
                item = Instantiate(fieldItemsSOonLevel[2].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.428f && vertColor < 0.571f)
                item = Instantiate(fieldItemsSOonLevel[3].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.571f && vertColor < 0.714f)
                item = Instantiate(fieldItemsSOonLevel[4].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.714f && vertColor < 0.857f)
                item = Instantiate(fieldItemsSOonLevel[5].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor == 1.0f)
                item = Instantiate(fieldItemsSOonLevel[6].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));

            PositioningItem(item);
        }
    }
    private void Instantiate_8_Items(FieldItemSO[] fieldItemsSOonLevel)
    {
        if (fieldItemsSOonLevel.Length != 8) Debug.LogError("Items count does not match. You've made a mistake");
        for (int i = 0; i < meshForPointsSource_8_Items.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource_8_Items.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color            
            if (!meshForPointsSource_8_Items.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source don't have vertex color attribute");
                return;
            }
            Transform item;
            float vertColor = meshForPointsSource_8_Items.colors[i].r;
            if (vertColor > 0.0f && vertColor < 0.125f)
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.125f && vertColor < 0.25f)
                item = Instantiate(fieldItemsSOonLevel[1].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.25f && vertColor < 0.375f)
                item = Instantiate(fieldItemsSOonLevel[2].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.375f && vertColor < 0.5f)
                item = Instantiate(fieldItemsSOonLevel[3].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.5f && vertColor < 0.625f)
                item = Instantiate(fieldItemsSOonLevel[4].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.625f && vertColor < 0.75f)
                item = Instantiate(fieldItemsSOonLevel[5].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.75f && vertColor < 0.875f)
                item = Instantiate(fieldItemsSOonLevel[6].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (vertColor > 0.875f && vertColor < 1.0f)
                item = Instantiate(fieldItemsSOonLevel[7].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
                item = Instantiate(fieldItemsSOonLevel[0].fieldItemPrefab, position, Quaternion.LookRotation(position));

            PositioningItem(item);
        }
    }

    private void PositioningItem(Transform item)
    {

        Vector3 turnItem = new(90.0f, 0.0f, 0.0f);
        item.eulerAngles += turnItem;
        item.Rotate(new(0.0f, UnityEngine.Random.value * 360f, 0.0f));
        item.position += GameManager.Instance.GameSessionData_.CurentPlanetPosition;
        Items.Add(item);
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

