using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptebaleObjects/HarvestersSO")]
public class HarvestersSO : ScriptableObject
{
    public GameObject harvesterPrefab;
    public GameObject harvesterSceneRefPrefab;
    public int price;
}
