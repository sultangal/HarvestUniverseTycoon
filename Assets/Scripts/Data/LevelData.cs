using UnityEngine;

public class LevelData 
{

    public FieldItemSO[] FieldItemsOnLevel { get; private set; }
    public int[] AmountOfCollectedFieldItemsOnLevel { get; private set; }
    public bool[] GoalAchievedFlags { get; private set; }

    public LevelData(FieldItemSO[] fieldItemSOs)
    {
        FieldItemsOnLevel = fieldItemSOs;
        AmountOfCollectedFieldItemsOnLevel = new int[fieldItemSOs.Length];
        GoalAchievedFlags = new bool[fieldItemSOs.Length];
    }

    public void AddCollectedAmountOfItems(int[] itemsCountArr)
    {
        if (!Planets.Instance.IsCurrPlanetActualLevel()) return;
        if (itemsCountArr.Length != AmountOfCollectedFieldItemsOnLevel.Length)
        {
            Debug.LogError("Array length does't the same.");
            return;
        }
        else
        {
            for (var i = 0; i < AmountOfCollectedFieldItemsOnLevel.Length; i++)
            {
                AmountOfCollectedFieldItemsOnLevel[i] += itemsCountArr[i];

                int itemAmountForNextLevel = Planets.Instance.GetCurrentLevelPlanetSO().fieldItemAmountForNextLevel[i];

                if (AmountOfCollectedFieldItemsOnLevel[i] > itemAmountForNextLevel)
                {
                    AmountOfCollectedFieldItemsOnLevel[i] = itemAmountForNextLevel;
                    GoalAchievedFlags[i] = true;
                }

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
}
