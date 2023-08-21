using UnityEngine;

public class HarvesterCollideLogic : MonoBehaviour
{
    [SerializeField] private ParticleSystem partSystem;
    private void OnTriggerEnter(Collider other)
    {
        var main = partSystem.main;
        if (other.gameObject.TryGetComponent(out FieldItemVisuals fieldItemVisuals))
        {
            main.startColor = fieldItemVisuals.ColorForVehicleParticles;
            GameManager.Instance.AddCash(other.gameObject);
        }
        if (other.gameObject.layer == 13)
        {
            GameManager.Instance.AddGold(other.gameObject);
        }
        Destroy(other.gameObject);             
        partSystem.Play();
    }
}
