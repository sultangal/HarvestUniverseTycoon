using UnityEngine;

public class VehicleCollideLogic : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        Destroy(other.gameObject);
        GameManager.Instance.AddScore();
        particleSystem.Play();
    }
}
