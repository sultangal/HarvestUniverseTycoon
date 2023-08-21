using UnityEngine;

public class GameSessionData
{
    public int collectedCash = 0;
    public int collectedGold = 0;
    public FieldItemSO[] FieldItemSOs { get; private set; } = null;
    public int[] CollectedFieldItemSOs { get; private set; } = null;
    public Vector3 CurentPlanetPosition { get; private set; } = Vector3.zero;



    public void ResetAllData(FieldItemSO[] fieldItemSOs, Vector3 curentPlanetPosition)
    {
        collectedCash = 0;
        collectedGold = 0;
        this.FieldItemSOs = fieldItemSOs;
        this.CurentPlanetPosition = curentPlanetPosition;
        if (fieldItemSOs != null ) 
            CollectedFieldItemSOs = new int[fieldItemSOs.Length];
}
}
