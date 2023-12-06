using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SavingSystem
{
    private readonly string globalDataPath;
    private readonly string planetDataPath;

    [Serializable]
    public class PlanetsDataObjectWrapperForSaving
    {
        public PlanetData[] PlanetsData;
    }

    PlanetsDataObjectWrapperForSaving save;

    public SavingSystem() {
        globalDataPath = Application.persistentDataPath + "/globalData.bin";
        planetDataPath = Application.persistentDataPath + "/planetData.bin";
        save = new();
    }

    public void SaveGlobalDataToFile(GlobalData data)
    {
        BinaryFormatter formatter = new();
        FileStream stream = new(globalDataPath, FileMode.Create);
        formatter.Serialize(stream, data);
        stream.Close();
        //File.WriteAllText(dataPath, JsonUtility.ToJson(data));
    }

    public void SavePlanetDataToFile(PlanetData[] data)
    {
        save.PlanetsData = data;
        BinaryFormatter formatter = new();
        FileStream stream = new(planetDataPath, FileMode.Create);
        formatter.Serialize(stream, save);
        stream.Close();
        //File.WriteAllText(dataPath, JsonUtility.ToJson(data));
    }

    public GlobalData LoadGlobalDataFromFile()
    {
        if (File.Exists(globalDataPath))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(globalDataPath, FileMode.Open);
            GlobalData data = formatter.Deserialize(stream) as GlobalData;
            stream.Close();
            //data = JsonUtility.FromJson<T>(File.ReadAllText(dataPath));
            return data;
        }
        else
        {
            return null;
        }
    }

    public PlanetData[] LoadPlanetDataFromFile()
    {
        if (File.Exists(planetDataPath))
        {
            BinaryFormatter formatter = new();
            FileStream stream = new(planetDataPath, FileMode.Open);
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

}
