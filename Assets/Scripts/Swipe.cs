using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Swipe : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Button btnPlay;
    private bool isRight;
    public void OnBeginDrag(PointerEventData eventData)
    {
        DOTween.KillAll();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var transform = HarvesterMovementControl.Instance.gameObject.transform;
        transform.localPosition = 
            new(transform.localPosition.x - eventData.delta.x * 0.001f, transform.localPosition.y, transform.localPosition.z);

        isRight = eventData.delta.x < 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Planets.Instance.IsFirstPlanet() && !isRight)
        {
            HarvesterMovementControl.Instance.MoveToInitialPosition();
            return;
        }

        if (Planets.Instance.IsLastPlanet() && isRight)
        {
            HarvesterMovementControl.Instance.MoveToInitialPosition();
            return;
        }

        if (isRight)
        {
            Planets.Instance.ShiftPlanetRight();
        }
        else
        {
            Planets.Instance.ShiftPlanetLeft();        
        }
        MainMenuUI.Instance.UpdatePlayButtonAvailability();
    }
}
