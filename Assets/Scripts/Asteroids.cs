using System.Collections;
using UnityEngine;


public class Asteroids : MonoBehaviour
{
    [SerializeField] private Transform AsteroidPrefab;
    [SerializeField] private float respawnPointRemoteness = 30f;   

    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (!TryGetComponent(out Planets planet))
        {
            Debug.LogError("Planets script not founded. In order to work properly, gameObject has to reference Planets script.");
            return;
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            StartCoroutine(AsteroidsFallCoroutine(planet.GetCurrentPlanet().position));
        }       
        else
        {
            StopAllCoroutines();
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

            yield return new WaitForSeconds((float)random.Next(0, 5));
        }

    }



    
}
