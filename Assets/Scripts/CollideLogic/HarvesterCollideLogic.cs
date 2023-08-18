using UnityEngine;

public class HarvesterCollideLogic : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSystem;
    private void OnTriggerEnter(Collider other)
    {
        var main = partSystem.main;
        if (other.gameObject.TryGetComponent(out FieldItemVisuals fieldItemVisuals))
            main.startColor = fieldItemVisuals.ColorForVehicleParticles;
        GameManager.Instance.AddCash(other.gameObject);
        Destroy(other.gameObject);             
        partSystem.Play();
    }
}
