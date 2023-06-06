using UnityEngine;

public class CollideBend : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Bend itemBend))
        {
            itemBend.isBend = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Bend itemBend))
        {
            itemBend.isBend = false;
        }
    }
}
