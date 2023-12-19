using System;
using System.IO;
using System.Linq;
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
    private static readonly DirectoryInfo globalDataDir;
    private static readonly DirectoryInfo planetDataDir;
    private static readonly DirectoryInfo harvesterDataDir;

    static SavingSystem()
    {
        globalDataDir = Directory.CreateDirectory(globalDataPath);
        planetDataDir = Directory.CreateDirectory(planetDataPath);
        harvesterDataDir = Directory.CreateDirectory(harvesterDataPath);
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
        WriteDataToFile(globalDataPath, "globalDataPath", data, ref globalDataVersion);
    }

    private static void SavePlanetDataToFile(PlanetData[] data)
    {
        PlanetsDataObjectWrapperForSaving save = new()
        {
            PlanetsData = data
        };
        WriteDataToFile(planetDataPath, "planetDataPath", save, ref planetDataVersion);
    }

    private static void SaveHarvesterData(bool[] data, int currAvailablePrefabIndex)
    {
        HarvesterDataObjectWrapperForSaving save = new()
        {
            HarvesterData = data,
            CurrAvailablePrefabIndex = currAvailablePrefabIndex
        };
        WriteDataToFile(harvesterDataPath, "harvesterDataPath", save, ref harvesterDataVersion);
    }

    private static void WriteDataToFile(string path, string filename, object data, ref int version)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(path + "/" + filename + "_" + version + ".harv", FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        
        
        if (version < numberOfReserveSaves-1)
            version++;
        else
            version = 0;
    }

    public static T LoadDataFromFile<T>()
    {
        FileInfo[] dirFiles = null;

        if (typeof(T) == typeof(GlobalData))
        {
            dirFiles = globalDataDir.GetFiles();
        }
        else
        if (typeof(T) == typeof(PlanetData[]))
        {
            dirFiles = planetDataDir.GetFiles();
        }
        else
        if (typeof(T) == typeof(HarvesterDataObjectWrapperForSaving))
        {
            dirFiles = harvesterDataDir.GetFiles();
        }
        else
            return default;

        if (dirFiles.Length == 0) return default;

        BinaryFormatter formatter = new();
        foreach (var item in dirFiles.OrderByDescending(f => f.LastAccessTime))
        {
            if (item.Extension != ".harv") break;
            FileStream stream = new(item.FullName, FileMode.Open);         

            try
            {
                T data = (T)formatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            catch (Exception)
            {
                break;
            }
            finally
            {
                stream.Close();
            }
        }
        return default;
    }

    public static bool[] LoadHarvesterStoreData(ref int currAvailablePrefabIndex)
    {
        HarvesterDataObjectWrapperForSaving data;
        data = LoadDataFromFile<HarvesterDataObjectWrapperForSaving>();
        if (data == null) return null;
        currAvailablePrefabIndex = data.CurrAvailablePrefabIndex;
        return data.HarvesterData;
    }
}
