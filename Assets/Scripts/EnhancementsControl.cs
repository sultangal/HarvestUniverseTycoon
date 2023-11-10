using UnityEngine;

public class EnhancementsControl : MonoBehaviour
{
    public static EnhancementsControl Instance { get; private set; }

    [SerializeField] private Transform bladeEnhance;
    [SerializeField] private Transform speedEnhance;
    [SerializeField] private Transform somethingEnhance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one EnhancementsControl!");
            return;
        }
    }

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        SetActiveForUI(true);
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            SetActiveForUI(true);
        }
    }

    private void SetActiveForUI(bool isActive)
    {
        bladeEnhance.gameObject.SetActive(isActive);
        speedEnhance.gameObject.SetActive(isActive);
        somethingEnhance.gameObject.SetActive(isActive);
    }
}
