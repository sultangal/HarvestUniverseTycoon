using DG.Tweening.Plugins.Core.PathCore;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingSystem
{
    private static readonly string globalDataPath = Application.persistentDataPath + "/globalData.bin";
    private static readonly string planetDataPath = Application.persistentDataPath + "/planetData.bin";
    private static readonly string harvesterDataPath = Application.persistentDataPath + "/harvesterData.bin";

    [Serializable]
    private class PlanetsDataObjectWrapperForSaving
    {
        public PlanetData[] PlanetsData;
    }

    [Serializable]
    private class HarvesterDataObjectWrapperForSaving
    {
        public bool[] HarvesterData;
        public int CurrAvailablePrefabIndex;
    }

    public static void SaveGame()
    {
        SaveGlobalDataToFile(GameManager.Instance.GlobalData_);
        SavePlanetDataToFile(Planets.Instance.PlanetData);
        var storeInst = Store.Instance;
        SaveHarvesterData(storeInst.HarvestersBought, storeInst.currAvailablePrefabIndex);
    }

    private static void SaveGlobalDataToFile(GlobalData data)
    {
        WriteDataToFile(globalDataPath, data);
    }

    private static void SavePlanetDataToFile(PlanetData[] data)
    {
        PlanetsDataObjectWrapperForSaving save = new()
        {
            PlanetsData = data
        };
        WriteDataToFile(planetDataPath, save);
    }

    private static void SaveHarvesterData(bool[] data, int currAvailablePrefabIndex)
    {
        HarvesterDataObjectWrapperForSaving save = new()
        {
            HarvesterData = data,
            CurrAvailablePrefabIndex = currAvailablePrefabIndex
        };
        WriteDataToFile(harvesterDataPath, save);
    }

    private static void WriteDataToFile(string path, object data)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(path, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GlobalData LoadGlobalDataFromFile()
    {
        if (File.Exists(globalDataPath))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(globalDataPath, FileMode.Open);
            GlobalData data = formatter.Deserialize(stream) as GlobalData;
            stream.Close();            
            return data;
            //data = JsonUtility.FromJson<T>(File.ReadAllText(dataPath));
        }
        else
        {
            return null;
        }
    }

    public static PlanetData[] LoadPlanetDataFromFile()
    {
        if (File.Exists(planetDataPath))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(planetDataPath, FileMode.Open);
            PlanetsDataObjectWrapperForSaving data = 
                formatter.Deserialize(stream) as PlanetsDataObjectWrapperForSaving;
            stream.Close();
            //data = JsonUtility.FromJson<T>(File.ReadAllText(dataPath));
            return data.PlanetsData;
        }
        else
        {
            return null;
        }
    }

    public static bool[] LoadHarvesterStoreData(ref int currAvailablePrefabIndex)
    {
        if (File.Exists(harvesterDataPath))
        {
            BinaryFormatter formatter = new();
            using FileStream stream = new(harvesterDataPath, FileMode.Open);
            HarvesterDataObjectWrapperForSaving data =
                formatter.Deserialize(stream) as HarvesterDataObjectWrapperForSaving;
            stream.Close();
            //data = JsonUtility.FromJson<T>(File.ReadAllText(dataPath));
            currAvailablePrefabIndex = data.CurrAvailablePrefabIndex;
            return data.HarvesterData;
        }
        else
        {
            return null;
        }
    }


}
