using System.Collections;
using UnityEngine;


public class AsteroidCollideLogic : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSystem;
    //[SerializeField] private GameObject asteroidMesh;
    public float moveSpeed;

    private IEnumerator moveToTarget;

    private void Start()
    {
        var main = partSystem.main;
        main.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
    }
    public void StartMoving(Vector3 target)
    {
        moveToTarget = MoveToTarget(target);
        StartCoroutine(moveToTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        StopCoroutine(moveToTarget);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90f, transform.eulerAngles.y, transform.eulerAngles.z);
        Asteroids.Instance.CreateCrater(transform.position, transform.rotation);
        Asteroids.Instance.CreateGold(transform.position, transform.rotation);
        StartCoroutine(StartDestroying());
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
            yield return null;
        }
    }

    private IEnumerator StartDestroying()
    {
        gameObject.GetComponent<Collider>().enabled = false;
        //asteroidMesh.SetActive(false);
        partSystem.Stop();
        yield return new WaitForSeconds(partSystem.main.duration);
        Destroy(gameObject);
    }
}
