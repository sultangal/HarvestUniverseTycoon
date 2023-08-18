using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptebaleObjects/PlanetSO")]
public class PlanetSO : ScriptableObject
{
    public string planetName;
    public FieldItemSO[] fieldItemSOs;
    public Color planetColor;
    public Transform planetPrefab;
}
