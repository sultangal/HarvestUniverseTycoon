using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Field : MonoBehaviour
{
    public Mesh meshForPointsSource;

    public List<Transform> Items { get; private set; }

    private readonly System.Random random = new();
    private readonly float randomMultiplier = 0.2f;
    //private bool firstInstantiation = true;
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        Items = new();
        //InstantiateFieldItems();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            DestroyFieldItems();
            if (!TryGetComponent(out Planets currPlanet))
            {
                Debug.LogError("Planets script not founded. In order to work properly, gameObject has to reference Planets script.");
            }
            InstantiateFieldItems(currPlanet.planetsArr[currPlanet.currentPlanetIndex].fieldItemSO.itemPrefab,
                currPlanet.planetsArr[currPlanet.currentPlanetIndex].planetRef.position);
        }
    

        //if (GameManager.Instance.IsGamePlaying())
        //{
        //    if (!firstInstantiation)
        //    {
        //        DestroyFieldItems();
        //        InstantiateFieldItems();
        //    }
        //    firstInstantiation = false;
        //}
    }

    public void InstantiateFieldItems(Transform itemRef, Vector3 planetPosition)
    {
        foreach (var vertex in meshForPointsSource.vertices)
        {
            Vector3 position = new((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble());
            position *= randomMultiplier;
            position += vertex;
            position.Normalize();
            position.Scale(new(5f, 5f, 5f));          
            Transform item = Instantiate(itemRef, position, Quaternion.LookRotation(position));
            Vector3 turnItem = new(90.0f, 0.0f, 0.0f);
            item.eulerAngles += turnItem;
            item.Rotate(new(0.0f, (float)random.NextDouble() * 100, 0.0f));
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

