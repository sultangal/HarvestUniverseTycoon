using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class TouchRotation : MonoBehaviour, IDragHandler
{
    [SerializeField] private Transform gameObjectToAffect;
    public void OnDrag(PointerEventData eventData)
    {
        float delta = eventData.delta.x;
        Vector3 eulerAngles = gameObjectToAffect.eulerAngles;
        gameObjectToAffect.Rotate(new(eulerAngles.x, eulerAngles.y + delta/100000000000000000000000f, eulerAngles.z));
        Debug.Log(delta / 100000000000000000000000f);
    }
}
