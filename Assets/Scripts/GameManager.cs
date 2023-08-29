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
    public event EventHandler<OnOnLevelUpEventArgs> OnLevelUp;
    public class OnOnLevelUpEventArgs : EventArgs
    {
        public int level;
    }

    public enum GameState
    {
        WaitingToStart,
        GameSessionPlaying,
        TimeIsUp,
        GameOver
    }
    private GameState state;    
    
    public GlobalData GlobalData_ { get; private set; } = new();
    public LevelData LevelData_ { get; private set; }
    public float COUNTDOWN_TIME = 23f;
    
    private bool countdownRunning = false;
    private bool isNewLevelFlag = false;

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
        InitializeAllData();
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

    private void InitializeAllData()
    {
        if (!TryGetComponent(out Field field))
        {
            Debug.LogError("No Fileds script attached!");
            return;
        }
        GlobalData_.pointsQuantity = field.meshForPointsSource.vertices.Length;
        LevelData_ = new LevelData(Planets.Instance.GetCurrentLevelPlanetSO().fieldItemSOs);
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

    private void UpdateData()
    {
        GlobalData_.amountOfCash += GameSessionData_.collectedCash;
        GlobalData_.amountOfGold += GameSessionData_.collectedGold;
        LevelData_.AddCollectedAmountOfItems(GameSessionData_.CollectedFieldItems);
    }

    private void CheckForNextLevel()
    {
        if (LevelData_.CheckIfNextLevelGoalAchieved())
        {
            GlobalData_.level++;
            LevelData_ = new LevelData(Planets.Instance.GetCurrentLevelPlanetSO().fieldItemSOs);
            OnLevelUp?.Invoke(this, new OnOnLevelUpEventArgs { level = GlobalData_.level });
            isNewLevelFlag = true;
        }
    }

    private void DemonstrateNewLevelIfAvailable()
    {
        if (isNewLevelFlag)
        {
            isNewLevelFlag = false;
            Planets.Instance.ShiftPlanetRight();
        }
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
                GameSessionData_.Reinitialize(Planets.Instance.GetCurrentPlanetSO().fieldItemSOs, Planets.Instance.GetCurrentPlanetSO().planetPrefab.position);
                StartCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.TimeIsUp:
                UpdateData();
                CheckForNextLevel();
                GameSessionData_.Reset();
                StopCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.GameOver:
                GameSessionData_.Reset();
                StopCountdown();
                this.state = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.WaitingToStart:
                DemonstrateNewLevelIfAvailable();
                GameSessionData_.Reset();
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
        if (GameSessionData_.FieldItemsOnLevel == null) return;
        GameSessionData_.collectedCash++;
        for (int i = 0; i < GameSessionData_.FieldItemsOnLevel.Length; i++)
        {
            if (ReferenceEquals(GameSessionData_.FieldItemsOnLevel[i].fieldItemPrefab.gameObject,
                gameObject.GetComponent<GameObjectReference>().gameObjRef))
            {
                GameSessionData_.CollectedFieldItems[i]++;
            }
        }
        OnCashAmountChanged?.Invoke(this, EventArgs.Empty);

        //WriteScoreToFile();

        //sif (IsScoreAchieved())
        //s{
        //s    TimeIsUp();
        //s}
    }

    public void AddGold()
    {
        GameSessionData_.collectedGold++;
    }
}
