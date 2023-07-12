using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform meshForPointsSource;
    [SerializeField] private Countdown countdown;

    [SerializeField] private PlanetSO[] planets;

    [SerializeField] public PlanetMeshSO planetMeshSO;

    public event EventHandler OnScoreChanged;
    public event EventHandler OnGameStateChanged;
    public event EventHandler OnPlanetShift;


    public enum GameState
    {
        WaitingToStart,
        GameSessionPlaying,
        GameSessionEnd
    }
    private GameState state;
    public static GameManager Instance { get; private set; }
    private PlayerData playerData;

#if UNITY_EDITOR
    //private readonly static string appPath = Application.dataPath;
#else
   // private readonly static string appPath = Application.persistentDataPath;
#endif

    //private readonly string scoreFilePath = appPath + "/score.json";



    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GameManager!");
            return;
        }
        Instance = this;

        playerData = new PlayerData
        {
            planets = planets
        };

        //ReadScoreFromFile();

    }
    private void Start()
    {
        PlanetController planetControl = this.AddComponent<PlanetController>();
        planetControl.CreatePlanets(ref playerData.planets, ref planetMeshSO);

        countdown.OnTimeIsUp += Countdown_OnTimeIsUp;
        playerData.pointsQuantity = meshForPointsSource.GetComponent<MeshFilter>().mesh.vertices.Length;
        Debug.Log("points quantity: " + playerData.pointsQuantity);
        SetGameState(GameState.WaitingToStart);
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
        //OnPlanetShift?.Invoke(this, EventArgs.Empty);

    }

    private void Countdown_OnTimeIsUp(object sender, EventArgs e)
    {
        GameSessionEnded();
    }

    public int GetScore()
    {
        return playerData.score;
    }

    public void AddScore()
    {
        playerData.score++;
        OnScoreChanged?.Invoke(this, EventArgs.Empty);
        //WriteScoreToFile();

        if (IsScoreAchieved())
        {
            GameSessionEnded();
        }
    }

    private void WriteScoreToFile()
    {
        //File.WriteAllText(scoreFilePath, JsonUtility.ToJson(playerData));
    }

    private void ReadScoreFromFile()
    {
        //playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(scoreFilePath));
    }

    public bool IsGamePlaying()
    {
        return state == GameState.GameSessionPlaying;
    }
    public bool IsGameWaitingToStart()
    {
        return state == GameState.WaitingToStart;
    }

    public bool IsGameSessionEnded()
    {
        return state == GameState.GameSessionEnd;
    }

    public void SetGameState(GameState state)
    {
        this.state = state;
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ShiftPlanetLeft()
    {
        if (playerData.currentPlanetIndex > 0)
        {
            playerData.currentPlanetIndex--;
            OnPlanetShift?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ShiftPlanetRight()
    {
        if (playerData.currentPlanetIndex < playerData.planets.Length)
        {
            playerData.currentPlanetIndex++;
            OnPlanetShift?.Invoke(this, EventArgs.Empty);
        }
    }

    public int GetCurrentPlanetIndex()
    {       
        return playerData.currentPlanetIndex;
    }

    private void GameSessionEnded()
    {
        playerData.score = 0;
        SetGameState(GameState.GameSessionEnd);       
    }

    private bool IsScoreAchieved()
    {
        return playerData.score == playerData.pointsQuantity;
    }


}
