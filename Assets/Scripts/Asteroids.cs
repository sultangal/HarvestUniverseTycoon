using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{
    public static Asteroids Instance { get; private set; }

    [SerializeField] private Transform asteroidPrefab;
    [SerializeField] private Transform cratorPrefab;
    [SerializeField] private float respawnPointRemoteness = 30f;
    [SerializeField] private float asteroidMoveSpeed = 9f;
    [SerializeField] private int minSecBetweenRespawn = 0;
    [SerializeField] private int maxSecBetweenRespawn = 1;
    
    private readonly List<GameObject> craters = new(); 
    private readonly List<GameObject> asteroids = new(); 
    private Planets planets;

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
        if (GameManager.Instance.IsGamePlaying())
        {
            StartCoroutine(AsteroidsFallCoroutine(planets.GetCurrentPlanetSO().planetPrefab.position));
        }
        else
            StopAllCoroutines();

        if (GameManager.Instance.IsGameWaitingToStart())
        {
            DestroyAsteroids();
            DestroyCraters();
        }
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
            asteroids.Add(asteriod.gameObject);
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
        foreach (var crater in craters)
        {
            Destroy(crater);
        }
        craters.Clear();
    }

    private void DestroyAsteroids()
    {
        foreach (var asteroid in asteroids)
        {
            if (asteroid != null)
            Destroy(asteroid);
        }
        asteroids.Clear();
    }

    public void CreateCrater(Vector3 position, Quaternion rotation)
    {
        Transform crater = Instantiate(cratorPrefab, position, rotation);
        craters.Add(crater.gameObject);
    }


}
