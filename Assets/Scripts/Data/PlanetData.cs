using UnityEngine;

public class PlanetData 
{

    //public FieldItemSO[] FieldItemsOnPlanet { get; private set; }
    public int[] amountOfCollectedFieldItemsOnPlanet;
    public bool[] goalAchievedFlags;

    //public PlanetData(FieldItemSO[] fieldItemSOs)
    //{
    //    //FieldItemsOnPlanet = fieldItemSOs;
    //    AmountOfCollectedFieldItemsOnPlanet = new int[fieldItemSOs.Length];
    //    //GoalAchievedFlags = new bool[fieldItemSOs.Length];
    //}
    /*
    public void AddCollectedAmountOfItems(int[] itemsCountArr)
    {
        //if (!Planets.Instance.IsCurrPlanetActualLevel()) return;
        if (itemsCountArr.Length != AmountOfCollectedFieldItemsOnPlanet.Length)
        {
            Debug.LogError("Array length does't the same.");
            return;
        }

        for (var i = 0; i < AmountOfCollectedFieldItemsOnPlanet.Length; i++)
        {
            AmountOfCollectedFieldItemsOnPlanet[i] += itemsCountArr[i];

            int itemAmountForNextLevel = Planets.Instance.GetCurrentLevelPlanetSO().fieldItemAmountForNextLevel[i];

            if (AmountOfCollectedFieldItemsOnPlanet[i] > itemAmountForNextLevel)
            {
                AmountOfCollectedFieldItemsOnPlanet[i] = itemAmountForNextLevel;
                GoalAchievedFlags[i] = true;
            }

        }
        
    }
    


    public bool CheckIfNextLevelGoalAchieved()
    {
        foreach (var flag in GoalAchievedFlags)
        {
            if (!flag)            
                return false;           
        }
        return true;
    }

    */
}
