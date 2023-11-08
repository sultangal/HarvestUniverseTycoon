using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuFields : MonoBehaviour
{
    private List<Transform> menuFieldPlanets;
    private void Start()
    {
        menuFieldPlanets = new List<Transform>();
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        AddMenuFieldPlanetsToList(menuFieldPlanets);
        InstantiateMenuItems();
        SetMenuItemsAvailability();
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {           
            gameObject.SetActive(true);
            SetMenuItemsAvailability();
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            gameObject.SetActive(false);
        }
    }

    private void AddMenuFieldPlanetsToList(List<Transform> list)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            list.Add(transform.GetChild(i));
        }
    }

    private void InstantiateMenuItems()
    {
        var planetsSOArr = Planets.Instance.planetsSOArr;
        for (int i = 0; i < menuFieldPlanets.Count; i++)
        {
            foreach (Transform item in menuFieldPlanets[i])
            {
                int rnd = Random.Range(0, planetsSOArr[i].fieldItemSOs.Length);
                var instance = Instantiate(planetsSOArr[i].fieldItemSOs[rnd].fieldItemPrefab, item);
                foreach (var component in instance.GetComponents<Component>())
                {
                    if (component is not Transform && component is not FieldItemVisuals) Destroy(component);
                }
                var fieldItemVisuals = instance.GetComponent<FieldItemVisuals>();
                fieldItemVisuals.SetAvailabilityVisual(false);
            }
        }
    }

    private void SetMenuItemsAvailability()
    {
        foreach (Transform item in menuFieldPlanets[GameManager.Instance.GlobalData_.level])
        {
            var comp = item.GetChild(0).GetComponent<FieldItemVisuals>();
            comp.SetAvailabilityVisual(true);
        }
    }
}
