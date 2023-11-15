using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptebaleObjects/PlanetSO")]
public class PlanetSO : ScriptableObject
{
    public string planetName;
    public FieldItemSO[] fieldItemSOs;
    public int[] fieldItemAmountGoal;
    public int speedEnhanceCost;
    public int bladesEnhanceCost;
    public int shieldEnhanceCost;
    public float asteriodMoveSpeed;
    public int minSecBetweenSpawn;
    public int maxSecBetweenSpawn;
    public Color planetColor;
    public Transform planetPrefab;
}


