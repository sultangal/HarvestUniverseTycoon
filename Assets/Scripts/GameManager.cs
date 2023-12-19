using System;
using UnityEngine;
using static SavingSystem;


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
    public bool IsNewLevelFlag { get; private set; } = false;
    public GameState State { get; private set; }
    public GlobalData GlobalData_;
    private readonly float COUNTDOWN_TIME = 15f;
    private bool countdownRunning = false;
    private bool isLastLevel;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one GameManager!");
            return;
        }
        Instance = this;
        isLastLevel = false;


        GlobalData_ = LoadDataFromFile<GlobalData>();
        GlobalData_ ??= new(); 
    }

    private void Start()
    {
        SetGameState(GameState.WaitingToStart);
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
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

    private void UpdateData()
    {
        GlobalData_.amountOfCash += GameSessionData_.collectedCash;
        GlobalData_.amountOfGold += GameSessionData_.collectedGold;
        Planets.Instance.AddCollectedAmountOfItems(GameSessionData_.CollectedFieldItems);
    }

    private bool CheckForNextLevel()
    {
        var pln = Planets.Instance;
        if (!pln.IsCurrPlanetActualLevel()) return false;

        if (pln.CheckIfNextLevelGoalAchieved())
        {
            if (GlobalData_.level == Planets.Instance.LastPlanetIndex)
            {
                if (!isLastLevel)
                {
                    Debug.Log("Last level achieved! Congratulations!");
                    isLastLevel = true;
                }
                return false;
            }
            GlobalData_.level++;           
            OnLevelUp?.Invoke(this, new OnOnLevelUpEventArgs { level = GlobalData_.level });
            IsNewLevelFlag = true;           
        }
        return true;
    }
    private bool TryWithdrawCash(int amount)
    {
        if (GlobalData_.amountOfCash >= amount)
        {
            GlobalData_.amountOfCash -= amount;
            OnCashAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
            return false;
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
                IsNewLevelFlag = false;
                GameSessionData_.Reinitialize(Planets.Instance.GetCurrentPlanetSO().fieldItemSOs, 
                    Planets.Instance.GetCurrentPlanetSO().planetPrefab.position);
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
                SavingSystem.SaveGame();
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.GameOver:
                GameSessionData_.Reset();
                StopCountdown();
                this.State = state;
                OnGameStateChanged?.Invoke(this, EventArgs.Empty);
                return;
            case GameState.WaitingToStart:             
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
    }

    public void AddGold()
    {
        GameSessionData_.collectedGold++;
        OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TryWithdrawBladesCost()
    {
        int cost = Planets.Instance.GetCurrLevelBladesEnhanceCost();
        if (TryWithdrawCash(cost))
            return true;
        else
            return false;
    }

    public bool TryWithdrawSpeedCost()
    {
        int cost = Planets.Instance.GetCurrLevelSpeedEnhanceCost();
        if (TryWithdrawCash(cost))
            return true;
        else 
            return false;
    }

    public bool TryWithdrawShieldCost()
    {
        int cost = Planets.Instance.GetCurrLevelShieldEnhanceCost();
        if (TryWithdrawCash(cost))
            return true;
        else
            return false;
    }

    public bool TryWithdrawGold(int amount)
    {
        if (GlobalData_.amountOfGold >= amount)
        {
            GlobalData_.amountOfGold -= amount;
            OnGoldAmountChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
            return false;
    }


}
