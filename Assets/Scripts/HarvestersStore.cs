using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestersStore : MonoBehaviour
{
    public static HarvestersStore Instance { get; private set; }

    public Transform[] harvestersPrefabArr;

    public EventHandler<OnUpdateHarvesterPrefabArgs> OnUpdateHarvesterPrefab;

    private Transform currentPrefab;

    public class OnUpdateHarvesterPrefabArgs : EventArgs
    {
        public Transform prefab;
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one HarvestersStore!");
            return;
        }
        Instance = this;
        currentPrefab = harvestersPrefabArr[0];
    }

    private void Start()
    {  
        //OnUpdateHarvesterPrefabArgs args = new()
        //{
        //    prefab = currentPrefab
        //};
        //OnUpdateHarvesterPrefab?.Invoke(this, args);
    }

    public Transform GetCurrentPrefab()
    {
        return currentPrefab;
    }

}
