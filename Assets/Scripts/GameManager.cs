using System;
using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Countdown countdown;   

    public event EventHandler OnScoreChanged;
    public event EventHandler OnGameStateChanged;   

    public enum GameState
    {
        WaitingToStart,
        GameSessionPlaying,
        TimeIsUp,
        GameOver
    }
    private GameState state;    
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
        playerData = new PlayerData();  
        //ReadScoreFromFile();

    }
    private void Start()
    {
        countdown.OnTimeIsUp += Countdown_OnTimeIsUp;
        if (!TryGetComponent(out Field field))
        {
            Debug.LogError("No Fileds script attached!");
            return; 
        }
        
        playerData.pointsQuantity = field.meshForPointsSource.vertices.Length;
        SetGameState(GameState.WaitingToStart);
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Countdown_OnTimeIsUp(object sender, EventArgs e)
    {
        TimeIsUp();
    }

    private void WriteScoreToFile()
    {
        //File.WriteAllText(scoreFilePath, JsonUtility.ToJson(playerData));
    }

    private void ReadScoreFromFile()
    {
        //playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(scoreFilePath));
    }

    private void TimeIsUp()
    {
        playerData.score = 0;
        SetGameState(GameState.TimeIsUp);       
    }

    private bool IsScoreAchieved()
    {
        return playerData.score == playerData.pointsQuantity;
    }

    public void SetGameState(GameState state)
    {
        this.state = state;
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsGamePlaying()
    {
        return state == GameState.GameSessionPlaying;
    }

    public bool IsGameWaitingToStart()
    {
        return state == GameState.WaitingToStart;
    }

    public bool IsTimeIsUp()
    {
        return state == GameState.TimeIsUp;
    }

    public bool IsGameOver()
    {
        return state == GameState.GameOver;
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
            TimeIsUp();
        }
    }

    public GameState GetState()
    {
        return state;
    }
}
