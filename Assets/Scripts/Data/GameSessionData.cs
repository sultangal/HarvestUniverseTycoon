using UnityEngine;

public class GameSessionData
{
    public int collectedCash = 0;
    public int collectedGold = 0;
    public FieldItemSO[] fieldItemSOs = null;
    public Vector3 curentPlanetPosition = Vector3.zero;

    public void ResetAllData()
    {
        collectedCash = 0;
        collectedGold = 0;
        FieldItemSO[] fieldItemSOs = null;
        Vector3 curentPlanetPosition = Vector3.zero;
}
}
