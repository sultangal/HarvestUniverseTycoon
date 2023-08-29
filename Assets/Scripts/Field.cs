using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Field : MonoBehaviour
{
    public Mesh meshForPointsSource;

    public List<Transform> Items { get; private set; } = new();
    private readonly float randomMultiplier = 0.02f;
    private void Start()
    {
        //if (!TryGetComponent(out planets))
        //{
        //    Debug.LogError("Planets script not founded. In order to work properly, gameObject has to reference Planets script.");
        //    return;
        //}

        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            DestroyFieldItems();
            //TODO: make inst depend on item quantity. Modify vertex color logic for that
            
            InstantiateFieldItems(GameManager.Instance.GameSessionData_.FieldItemsOnLevel,
                GameManager.Instance.GameSessionData_.CurentPlanetPosition);
        }

        if (GameManager.Instance.IsGameWaitingToStart())
        {
            DestroyFieldItems();
        }
    }

    public void InstantiateFieldItems(FieldItemSO[] fieldItemSOsArr, Vector3 planetPosition)
    {
        for (int i = 0; i < meshForPointsSource.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color
            
            if (!meshForPointsSource.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            {
                Debug.LogError("Mesh for point source dont have vertex color attribute");
                return;
            }
            Transform item;
            if (meshForPointsSource.colors[i].r > 0.0f && meshForPointsSource.colors[i].r < 0.125f)
                item = Instantiate(fieldItemSOsArr[0].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (meshForPointsSource.colors[i].r > 0.125f && meshForPointsSource.colors[i].r < 0.25f)
                item = Instantiate(fieldItemSOsArr[1].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (meshForPointsSource.colors[i].r > 0.25f && meshForPointsSource.colors[i].r < 0.375f)
                item = Instantiate(fieldItemSOsArr[2].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (meshForPointsSource.colors[i].r > 0.375f && meshForPointsSource.colors[i].r < 0.5f)
                item = Instantiate(fieldItemSOsArr[3].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (meshForPointsSource.colors[i].r > 0.5f && meshForPointsSource.colors[i].r < 0.625f)
                item = Instantiate(fieldItemSOsArr[4].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (meshForPointsSource.colors[i].r > 0.625f && meshForPointsSource.colors[i].r < 0.75f)
                item = Instantiate(fieldItemSOsArr[5].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (meshForPointsSource.colors[i].r > 0.75f && meshForPointsSource.colors[i].r < 0.875f)
                item = Instantiate(fieldItemSOsArr[6].fieldItemPrefab, position, Quaternion.LookRotation(position));
            else
            if (meshForPointsSource.colors[i].r > 0.875f && meshForPointsSource.colors[i].r < 1.0f)
                item = Instantiate(fieldItemSOsArr[7].fieldItemPrefab, position, Quaternion.LookRotation(position));            
            else 
                item = Instantiate(fieldItemSOsArr[0].fieldItemPrefab, position, Quaternion.LookRotation(position));

            //save reference to original prefab to identify this clone later
            item.GetComponent<GameObjectReference>().gameObjRef = fieldItemSOsArr[0].fieldItemPrefab.gameObject;



            Vector3 turnItem = new(90.0f, 0.0f, 0.0f);
            item.eulerAngles += turnItem;
            item.Rotate(new(0.0f, UnityEngine.Random.value * 360f, 0.0f));
            item.position += planetPosition;
            Items.Add(item);
        }
        /*
        for (int i = 0; i < meshForPointsSource.vertices.Length; i++)
        {
            Vector3 position = new(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);
            position *= randomMultiplier;
            position += meshForPointsSource.vertices[i];
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));

            //ditributing items according to mesh vertex color
            Transform item;
            //if (!meshForPointsSource.HasVertexAttribute(UnityEngine.Rendering.VertexAttribute.Color))
            //{
            //    Debug.LogError("Mesh for point source dont have vertex color attribute");
            //    return;
            //} else
            //{
            
                int rndIndex = UnityEngine.Random.Range(0, fieldItemSOsArr.Length);
            //if (meshForPointsSource.colors[i].r > 0.5f)
            item = Instantiate(fieldItemSOsArr[rndIndex].fieldItemPrefab, position, Quaternion.LookRotation(position));
            //save reference to original prefab to identify this clone later
            item.GetComponent<GameObjectReference>().gameObjRef = fieldItemSOsArr[rndIndex].fieldItemPrefab.gameObject;
            //else
            //item = Instantiate(itemRef2, position, Quaternion.LookRotation(position));
            //}

            Vector3 turnItem = new(90.0f, 0.0f, 0.0f);
            item.eulerAngles += turnItem;
            item.Rotate(new(0.0f, UnityEngine.Random.value * 360f, 0.0f));
            item.position += planetPosition;
            Items.Add(item);
        }
        */
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

