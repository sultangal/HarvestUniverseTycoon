using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TouchRotation : MonoBehaviour, IDragHandler
{
    private Transform gameObjectToAffect;

    private void Start()
    {
        Store.Instance.OnUpdateHarvesterPrefab += StoreManager_OnUpdateHarvesterPrefab;
    }

    private void StoreManager_OnUpdateHarvesterPrefab(object sender, Store.OnUpdateHarvesterPrefabArgs e)
    {
        gameObjectToAffect = e.prefab;
    }

    private void OnEnable()
    {
        gameObjectToAffect = Store.Instance.GetCurrentPrefab();
    }

    public void OnDrag(PointerEventData eventData)
    {
        float delta = eventData.delta.x*0.3f;
        Vector3 eulerAngles = gameObjectToAffect.eulerAngles;
        gameObjectToAffect.Rotate(new(eulerAngles.x, -delta, eulerAngles.z));
    }
}
