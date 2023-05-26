using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform meshForPointsSource;

    public event EventHandler OnScoreChanged;
    public event EventHandler OnGameStateChanged;

    public enum GameState
    {
        WaitingToStart,
        GamePlaying,
        GameEnd
    }
    private GameState state;
    public static GameManager Instance { get; private set; }
    private PlayerData playerData;
#if UNITY_EDITOR
    private readonly static string appPath = Application.dataPath;
#else
    private readonly static string appPath = Application.persistentDataPath;
#endif

    private readonly string scoreFilePath = appPath + "/score.json";

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GameManager!");
            return;
        }
        Instance = this;

        playerData = new PlayerData();
        //ReadScoreFromFile();

    }
    private void Start()
    {
        playerData.pointsQuantity = meshForPointsSource.GetComponent<MeshFilter>().mesh.vertices.Length;
        Debug.Log("points quantity: " + playerData.pointsQuantity);
        SetGameState(GameState.WaitingToStart);
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
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
            SetGameState(GameState.GameEnd);
    }

    private void WriteScoreToFile()
    {
        File.WriteAllText(scoreFilePath, JsonUtility.ToJson(playerData));
    }

    private void ReadScoreFromFile()
    {
        playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(scoreFilePath));
    }

    public bool IsGamePlaying()
    {
        return state == GameState.GamePlaying;
    }
    public bool IsGameWaitingToStart()
    {
        return state == GameState.WaitingToStart;
    }

    public bool IsGameEnd()
    {
        return state == GameState.GameEnd;
    }

    public void SetGameState(GameState state)
    {
        this.state = state;
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }


    private bool IsScoreAchieved()
    {
        return playerData.score == playerData.pointsQuantity;
    }

    private class PlayerData
    {
        public int score = 0;
        public int pointsQuantity = 0;
    }
}
