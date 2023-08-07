using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;


public class Asteroid : MonoBehaviour
{
    [SerializeField] Transform AsteroidPrefab;
    private List<GameObject> cubes;
    private void Start()
    {
        //cubes = new();
        //for (int i = 0; i < 50; i++)
        //{
        //    Vector3 point = Random.onUnitSphere;
        //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //    cube.transform.SetPositionAndRotation(point * 30, Quaternion.LookRotation(point));
        //    cubes.Add(cube);
        //}

        StartCoroutine(AsteroidFallCoroutine());

  
    }

    private IEnumerator AsteroidFallCoroutine()
    {
        while(true)
        {
            System.Random random = new();
            Vector3 instPoint = Random.onUnitSphere;
            Transform asteriod = Instantiate(AsteroidPrefab, instPoint, Quaternion.LookRotation(instPoint));
            asteriod.transform.SetPositionAndRotation(instPoint * 30, Quaternion.LookRotation(instPoint));
            //TODO: Make target point changeble to current planet
            StartCoroutine(MoveToTarget(asteriod, Vector3.zero));
            //cube.transform.DOMove(Vector3.zero, 5);
            yield return new WaitForSeconds((float)random.Next(0, 5));
        }

    }

    private IEnumerator MoveToTarget(Transform obj, Vector3 target)
    {
        while (obj.position != target)
        {
            obj.position = Vector3.MoveTowards(obj.position, target, Time.deltaTime*10f);
            yield return null;
        }
    }

}
