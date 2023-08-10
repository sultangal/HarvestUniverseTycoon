using UnityEngine;

public class VehicleCollideLogic : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSystem;
    private void OnTriggerEnter(Collider other)
    {

        //Debug.Log(other.gameObject);
        Destroy(other.gameObject);
        GameManager.Instance.AddScore();
        partSystem.Play();
    }
}
