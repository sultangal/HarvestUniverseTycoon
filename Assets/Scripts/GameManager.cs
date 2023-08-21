using System;
using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float CountdownTime { get; private set; }
    public GameSessionData GameSessionData { get; private set; } = new();

    public event EventHandler OnCashAmountChanged;
    public event EventHandler OnGameStateChanged;   

    public enum GameState
    {
        WaitingToStart,
        GameSessionPlaying,
        TimeIsUp,
        GameOver
    }
    private GameState state;    
    
    private readonly GlobalData globalData = new ();
    private readonly float COUNTDOWN_TIME = 59f;
    
    private bool countdownRunning = false;
    private Planets planets;

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
        //ReadScoreFromFile();

    }
    private void Start()
    {

        if (!TryGetComponent(out planets))
        {
            Debug.LogError("Planets script not founded. In order to work properly, gameObject has to reference Planets script.");
            return;
        }

        if (!TryGetComponent(out Field field))
        {
            Debug.LogError("No Fileds script attached!");
            return; 
        }

        globalData.pointsQuantity = field.meshForPointsSource.vertices.Length;
        SetGameState(GameState.WaitingToStart);
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
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
        GameSessionData.collectedCash = 0;
        SetGameState(GameState.TimeIsUp);       
    }

    private bool IsScoreAchieved()
    {
        return GameSessionData.collectedCash == globalData.pointsQuantity;
    }

    private void IterateCountdown()
    {
        if (countdownRunning)
        {
            CountdownTime -= Time.deltaTime;
            if (CountdownTime < 0)
            {
                CountdownTime = COUNTDOWN_TIME;
                TimeIsUp();
                countdownRunning = false;
            }
        }
    }
    private void StartCountdown()
    {
        CountdownTime = COUNTDOWN_TIME;
        countdownRunning = true;
    }
    private void StopCountdown()
    {
        countdownRunning = false;
    }

    private void Update()
    {
        IterateCountdown();
    }

    public void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.GameSessionPlaying:
                GameSessionData.ResetAllData(planets.GetCurrentPlanetSO().fieldItemSOs, planets.GetCurrentPlanetSO().planetPrefab.position);
                StartCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            default:
                GameSessionData.ResetAllData(null, Vector3.zero);
                StopCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
        }

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

    public int GetCashAmount()
    {
        return GameSessionData.collectedCash;
    }

    public void AddCash(GameObject gameObject)
    {
        GameSessionData.collectedCash++;
        for (int i = 0; i < GameSessionData.FieldItemSOs.Length; i++)
        {
            if (ReferenceEquals(GameSessionData.FieldItemSOs[i].fieldItemPrefab.gameObject,
                gameObject.GetComponent<GameObjectReference>().gameObjRef))
            {
                GameSessionData.CollectedFieldItemSOs[i]++;
            }
        }
        OnCashAmountChanged?.Invoke(this, EventArgs.Empty);
        //WriteScoreToFile();

        //sif (IsScoreAchieved())
        //s{
        //s    TimeIsUp();
        //s}
    }

    public void AddGold(GameObject gameObject)
    {
        GameSessionData.collectedGold++;
    }
    public GameState GetState()
    {
        return state;
    }


}
