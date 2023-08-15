using UnityEngine;

public class VehicleCollideLogic : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSystem;
    private void OnTriggerEnter(Collider other)
    {
        var main = partSystem.main;
        if (other.gameObject.TryGetComponent(out FieldItemVisuals fieldItemVisuals))
            main.startColor = fieldItemVisuals.ColorForVehicleParticles;

        Destroy(other.gameObject);
        GameManager.Instance.AddScore();        
        partSystem.Play();
    }
}
