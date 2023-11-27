using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Swipe : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject gameObjectToAffect;
    private bool isRight;
    private Material skyboxMat; 

    private void Start()
    {
        skyboxMat = RenderSettings.skybox;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        DOTween.Kill(10);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var transform = gameObjectToAffect.transform;
        float delta = eventData.delta.x * 0.001f;
        transform.localPosition = 
            new(transform.localPosition.x - delta, transform.localPosition.y, transform.localPosition.z);

        
        float df = skyboxMat.GetFloat("_Rotation");
        skyboxMat.SetFloat("_Rotation", df - delta);

        isRight = eventData.delta.x < 0;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Planets.Instance.IsFirstPlanet() && !isRight)
        {
            MenuControl.Instance.ReturnToInitialPosition();
            Skybox.Instance.ReturnRotation();
            return;
        }

        if (Planets.Instance.IsLastPlanet() && isRight)
        {
            MenuControl.Instance.ReturnToInitialPosition();
            Skybox.Instance.ReturnRotation();
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
