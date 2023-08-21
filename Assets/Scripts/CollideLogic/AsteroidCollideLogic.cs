using System.Collections;
using UnityEngine;


public class AsteroidCollideLogic : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSystem;
    public float moveSpeed;

    private IEnumerator moveToTarget;

    public void StartMoving(Vector3 target)
    {
        moveToTarget = MoveToTarget(target);
        StartCoroutine(moveToTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        StopCoroutine(moveToTarget);           
        StartCoroutine(StartDestroying());
        transform.eulerAngles = new Vector3(transform.eulerAngles.x + 90f, transform.eulerAngles.y, transform.eulerAngles.z);
        Asteroids.Instance.CreateCrater(transform.position, transform.rotation);
        Asteroids.Instance.CreateGold(transform.position, transform.rotation);

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
        gameObject.SetActive(false);
        partSystem.Stop();
        yield return new WaitForSeconds(partSystem.main.duration);
        Destroy(gameObject);
    }
}
