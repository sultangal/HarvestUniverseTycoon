using System;
using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float CountdownTime { get; private set; }
    public GameSessionData GameSessionData_ { get; private set; } = new();

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
    
    public GlobalData GlobalData_ { get; private set; } = new();
    public float COUNTDOWN_TIME = 23f;
    
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

        GlobalData_.pointsQuantity = field.meshForPointsSource.vertices.Length;
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
        SetGameState(GameState.TimeIsUp);       
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
                GameSessionData_.ResetAllData(planets.GetCurrentPlanetSO().fieldItemSOs, planets.GetCurrentPlanetSO().planetPrefab.position);
                StartCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.TimeIsUp:
                UpdateGlobalData();
                GameSessionData_.ResetAllData(null, Vector3.zero);
                StopCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.GameOver:
                GameSessionData_.ResetAllData(null, Vector3.zero);
                StopCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.WaitingToStart:
                GameSessionData_.ResetAllData(null, Vector3.zero);
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
        return GameSessionData_.collectedCash;
    }

    public void AddCash(GameObject gameObject)
    {
        GameSessionData_.collectedCash++;
        for (int i = 0; i < GameSessionData_.FieldItemSOs.Length; i++)
        {
            if (ReferenceEquals(GameSessionData_.FieldItemSOs[i].fieldItemPrefab.gameObject,
                gameObject.GetComponent<GameObjectReference>().gameObjRef))
            {
                GameSessionData_.CollectedFieldItemSOs[i]++;
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
        GameSessionData_.collectedGold++;
    }

    public void UpdateGlobalData()
    {
        GlobalData_.amountOfCash += GameSessionData_.collectedCash;
        GlobalData_.amountOfGold += GameSessionData_.collectedGold;
    }

    public GameState GetState()
    {
        return state;
    }


}
