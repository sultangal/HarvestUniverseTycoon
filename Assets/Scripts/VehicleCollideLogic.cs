using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VehicleCollideLogic : MonoBehaviour
{
    [SerializeField] private new ParticleSystem particleSystem;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        Destroy(other.gameObject);
        GameManager.Instance.AddScore();
        particleSystem.Play();
    }
}
