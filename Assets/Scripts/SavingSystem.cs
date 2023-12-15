using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SavingSystem
{
    private static readonly string globalDataPath = Application.persistentDataPath + "/globalData";
    private static readonly string planetDataPath = Application.persistentDataPath + "/planetData";
    private static readonly string harvesterDataPath = Application.persistentDataPath + "/harvesterData";
    private static readonly int numberOfReserveSaves = 5;
    private static int globalDataVersion = 0;
    private static int planetDataVersion = 0;
    private static int harvesterDataVersion = 0;

    static SavingSystem()
    {
        Directory.CreateDirectory(globalDataPath);
        Directory.CreateDirectory(planetDataPath);
        Directory.CreateDirectory(harvesterDataPath);
    }

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
        WriteDataToFile(globalDataPath, "globalDataPath.bin", data, ref globalDataVersion);
    }

    private static void SavePlanetDataToFile(PlanetData[] data)
    {
        PlanetsDataObjectWrapperForSaving save = new()
        {
            PlanetsData = data
        };
        WriteDataToFile(planetDataPath, "planetDataPath.bin", save, ref planetDataVersion);
    }

    private static void SaveHarvesterData(bool[] data, int currAvailablePrefabIndex)
    {
        HarvesterDataObjectWrapperForSaving save = new()
        {
            HarvesterData = data,
            CurrAvailablePrefabIndex = currAvailablePrefabIndex
        };
        WriteDataToFile(harvesterDataPath, "harvesterDataPath.bin", save, ref harvesterDataVersion);
    }


    //read about packing and unpacking
    private static void WriteDataToFile(string path, string filename, object data, ref int version)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(path + "/" + filename + "_" + version, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        if (version < numberOfReserveSaves-1)
            version++;
        else
            version = 0;
    }

    /*var myFile = directory.GetFiles()
             .OrderByDescending(f => f.LastWriteTime)
             .First();
    */

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
            HarvesterDataObjectWrapperForSaving data;
            //try
            //{
                data = formatter.Deserialize(stream) as HarvesterDataObjectWrapperForSaving;
                currAvailablePrefabIndex = data.CurrAvailablePrefabIndex;
                
            //}
            //catch (Exception)
            //{
            //    
            //}

            stream.Close();
            return data.HarvesterData;
        }
        else
        {
            return null;
        }
    }




}
