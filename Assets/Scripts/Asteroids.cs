using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    public static Asteroids Instance { get; private set; }

    [SerializeField] private Transform asteroidPrefab;
    [SerializeField] private Transform cratorPrefab;
    [SerializeField] private Transform goldPrefab;
    public float respawnPointRemoteness = 30f;
    public float asteroidMoveSpeed = 9f;
    public int minSecBetweenRespawn = 0;
    public int maxSecBetweenRespawn = 1;
    
    public List<GameObject> CratersList { get; private set; } = new(); 
    public List<GameObject> AsteroidsList { get; private set; } = new(); 
    public List<GameObject> GoldsList { get; private set; } = new(); 
    private Planets planets;

    private const float CHANCE_TO_INSTANTIATE = 0.3f;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Asteroids!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (!TryGetComponent(out planets))
        {
            Debug.LogError("Planets script not founded. In order to work properly, gameObject has to reference Planets script.");
            return;
        }
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        //if (GameManager.Instance.IsGamePlaying())
        //{
        //    StartCoroutine(AsteroidsFallCoroutine(planets.GetCurrentPlanetSO().planetPrefab.position));
        //}
        //else
        //    StopAllCoroutines();

        //if (GameManager.Instance.IsGameWaitingToStart())
        //{
        //    DestroyAsteroids();
        //    DestroyCraters();
        //    DestroyGolds();
        //}
    }

    private IEnumerator AsteroidsFallCoroutine(Vector3 target)
    {
        while(true)
        {
            System.Random random = new();
            Vector3 vecOnUnitSphere = Random.onUnitSphere;
            Vector3 respawnPoint = vecOnUnitSphere * respawnPointRemoteness;
            respawnPoint += target;
            Transform asteriod = Instantiate(asteroidPrefab, respawnPoint, Quaternion.LookRotation(vecOnUnitSphere));
            AsteroidsList.Add(asteriod.gameObject);
            if (asteriod.TryGetComponent(out AsteroidCollideLogic asteroidCL))
            {
                asteroidCL.moveSpeed = asteroidMoveSpeed;
                asteroidCL.StartMoving(target);
            }
            else
                Debug.LogError("AsteroidCollideLogic script not founded.");

            yield return new WaitForSeconds((float)random.Next(minSecBetweenRespawn, maxSecBetweenRespawn));
        }

    }

    private void DestroyCraters()
    {
        foreach (var crater in CratersList)
        {
            Destroy(crater);
        }
        CratersList.Clear();
    }

    private void DestroyAsteroids()
    {
        foreach (var asteroid in AsteroidsList)
        {
            if (asteroid != null)
            Destroy(asteroid);
        }
        AsteroidsList.Clear();
    }

    private void DestroyGolds()
    {
        foreach (var gold in GoldsList)
        {
            if (gold != null)
                Destroy(gold);
        }
        GoldsList.Clear();
    }

    public void CreateCrater(Vector3 position, Quaternion rotation)
    {
        Transform crater = Instantiate(cratorPrefab, position, rotation);
        CratersList.Add(crater.gameObject);
    }

    public void CreateGold(Vector3 position, Quaternion rotation)
    {   
        if (UnityEngine.Random.value < CHANCE_TO_INSTANTIATE)
        {
            Transform gold = Instantiate(goldPrefab, position, rotation);
            GoldsList.Add(gold.gameObject);
        }
    }


}
