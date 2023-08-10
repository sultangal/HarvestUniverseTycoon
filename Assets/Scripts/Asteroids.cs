using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroids : MonoBehaviour
{
    public static Asteroids Instance { get; private set; }

    [SerializeField] private Transform AsteroidPrefab;
    [SerializeField] private float respawnPointRemoteness = 30f;
    private List<GameObject> craters = new(); 
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
            Transform asteriod = Instantiate(AsteroidPrefab, respawnPoint, Quaternion.LookRotation(vecOnUnitSphere));

            if (asteriod.TryGetComponent(out AsteroidCollideLogic asteroidCL))
                asteroidCL.StartMoving(target);            
            else 
                Debug.LogError("AsteroidCollideLogic script not founded.");

            yield return new WaitForSeconds((float)random.Next(0, 2));
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

    public void CreateCrater(Vector3 position, Quaternion rotation)
    {
        GameObject crater = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        crater.transform.SetPositionAndRotation(position, rotation);
        craters.Add(crater);
    }


}
