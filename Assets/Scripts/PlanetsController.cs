using System;
using UnityEngine;

public class PlanetsController : MonoBehaviour
{
    public static PlanetsController Instance { get; private set; }

    [SerializeField] private PlanetSO[] planets;
    [SerializeField] private PlanetMeshSO planetMeshSO;

    public event EventHandler<OnPlanetShiftEventArgs> OnPlanetShift;

    public class OnPlanetShiftEventArgs : EventArgs
    {
        public Transform currPlanetTransform;
    }

    public int CurrentPlanetIndex { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one PlanetsController!");
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        CreatePlanets();
        SetPlanetsVisibility();
    }

    private void CreatePlanets()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].planetRef = Instantiate(
                planetMeshSO.planetMesh,
                planetMeshSO.planetMesh.position,
                planetMeshSO.planetMesh.rotation);
            
            Vector3 newPos = new(15.0f*(float)i, 0f, 0f);
            planets[i].planetRef.position += newPos;
            planets[i].planetRef.GetComponent<PlanetVisual>().SetPlanetColor(planets[i].planetColor);
        }
    }

    public void ShiftPlanetLeft()
    {
        if (CurrentPlanetIndex > 0)
        {
            CurrentPlanetIndex--;
            SetPlanetsVisibility();
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planets[CurrentPlanetIndex].planetRef });
        }
    }

    public void ShiftPlanetRight()
    {
        if (CurrentPlanetIndex < planets.Length-1)
        {
            CurrentPlanetIndex++;
            SetPlanetsVisibility();
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planets[CurrentPlanetIndex].planetRef });
        }
    }

    private void SetPlanetsVisibility()
    {
        //TODO: FIX visibility error
        for (int i = 0; i < planets.Length; i++)
        {
            if (CurrentPlanetIndex == i || CurrentPlanetIndex == i-- || CurrentPlanetIndex == i++)
            {
                planets[i].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = true;
                
            }
            else
                planets[i].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = false;

            //Debug.Log("Bitch" + i);
        }


    }
}
