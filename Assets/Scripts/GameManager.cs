using System;
using System.Collections;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float CountdownTime { get; private set; }
    public GameSessionData GameSessionData_ { get; private set; } = new();

    public event EventHandler OnCashAmountChanged;
    public event EventHandler OnGoldAmountChanged;
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
    public GameState State { get; private set; }

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
        //GlobalData_.pointsQuantity = field.meshForPointsSource.vertices.Length;
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

    private bool CheckForNextLevel()
    {
        if (LevelData_.CheckIfNextLevelGoalAchieved())
        {
            if (GlobalData_.level == Planets.Instance.LastPlanetIndex)
            {
                Debug.Log("Last level achieved! Congratulations!");
                return false;
            }
            GlobalData_.level++;           
            LevelData_ = new LevelData(Planets.Instance.GetCurrentLevelPlanetSO().fieldItemSOs);
            OnLevelUp?.Invoke(this, new OnOnLevelUpEventArgs { level = GlobalData_.level });
            isNewLevelFlag = true;           
        }
        return true;
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
                this.State = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.TimeIsUp:
                UpdateData();
                CheckForNextLevel();
                GameSessionData_.Reset();
                StopCountdown();
                this.State = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.GameOver:
                GameSessionData_.Reset();
                StopCountdown();
                this.State = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.WaitingToStart:
                DemonstrateNewLevelIfAvailable();
                GameSessionData_.Reset();
                StopCountdown();
                this.State = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
        }

    }

    public bool IsGamePlaying()
    {
        return State == GameState.GameSessionPlaying;
    }

    public bool IsGameWaitingToStart()
    {
        return State == GameState.WaitingToStart;
    }

    public bool IsTimeIsUp()
    {
        return State == GameState.TimeIsUp;
    }

    public bool IsGameOver()
    {
        return State == GameState.GameOver;
    }

    public int GetCashAmount()
    {
        return GameSessionData_.collectedCash;
    }

    public int GetGoldAmount()
    {
        return GameSessionData_.collectedGold;
    }

    public void AddCash(GameObject gameObject)
    {
        if (GameSessionData_.FieldItemsSOonLevel == null) return;
        GameSessionData_.collectedCash++;
        for (int i = 0; i < GameSessionData_.FieldItemsSOonLevel.Length; i++)
        {
            //compare refs to identify and count collected items
            if (ReferenceEquals(GameSessionData_.FieldItemsSOonLevel[i].fieldItemPrefab.gameObject,
                gameObject.GetComponent<GameObjectReference>().fieldItemSORef.fieldItemPrefab.gameObject))
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
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
    }
}
