using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AsteroidCollideLogic : MonoBehaviour
{
    public void StartMoving(Vector3 target)
    {
        StartCoroutine(MoveToTarget(target));
    }

    private void OnTriggerEnter(Collider other)
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator MoveToTarget(Vector3 target)
    {
        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 10f);
            yield return null;
        }
    }
}
