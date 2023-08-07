using System;
using UnityEngine;

public class Planets : MonoBehaviour
{
    public static Planets Instance { get; private set; }

    [SerializeField] private PlanetSO[] planets;
    [SerializeField] private PlanetMeshSO planetMeshSO;

    public event EventHandler<OnPlanetShiftEventArgs> OnPlanetShift;

    public class OnPlanetShiftEventArgs : EventArgs
    {
        public Transform currPlanetTransform;
    }

    private int currentPlanetIndex = 0;

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
        if (currentPlanetIndex > 0)
        {
            currentPlanetIndex--;
            SetPlanetsVisibility();
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planets[currentPlanetIndex].planetRef });
        }
    }

    public void ShiftPlanetRight()
    {
        if (currentPlanetIndex < planets.Length-1)
        {
            currentPlanetIndex++;
            SetPlanetsVisibility();
            OnPlanetShift?.Invoke(this, new OnPlanetShiftEventArgs { currPlanetTransform = planets[currentPlanetIndex].planetRef });
        }
    }

    public Transform GetCurrentPlanet()
    {
        return planets[currentPlanetIndex].planetRef;
    }

    private void SetPlanetsVisibility()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = false;
         
        }

        if (currentPlanetIndex > 0)
            planets[currentPlanetIndex - 1].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = true;

        planets[currentPlanetIndex].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = true;

        if (currentPlanetIndex < planets.Length - 1)
        {
            planets[currentPlanetIndex + 1].planetRef.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
