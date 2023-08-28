using UnityEngine;
using static GameSessionData;

public class GameSessionData
{
    public int collectedCash = 0;
    public int collectedGold = 0;

    public FieldItemSO[] FieldItemsOnLevel { get; private set; } = null;
    public int[] CollectedFieldItems { get; private set; } = null;
    public Vector3 CurentPlanetPosition { get; private set; } = Vector3.zero;



    public void Reinitialize(FieldItemSO[] fieldItemSOs, Vector3 curentPlanetPosition)
    {
        collectedCash = 0;
        collectedGold = 0;
        this.FieldItemsOnLevel = fieldItemSOs;
        this.CurentPlanetPosition = curentPlanetPosition;
        if (fieldItemSOs != null)
            CollectedFieldItems = new int[fieldItemSOs.Length];
    }

    public void Reset()
    {
        collectedCash = 0;
        collectedGold = 0;
        this.FieldItemsOnLevel = null;
        this.CurentPlanetPosition = Vector3.zero;
    }
}
