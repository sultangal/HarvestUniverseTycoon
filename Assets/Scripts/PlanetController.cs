using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlanetController : MonoBehaviour
{
    public void Start()
    {
        GameManager.Instance.OnPlanetShift += GameManager_OnPlanetShift;
    }

    private void GameManager_OnPlanetShift(object sender, System.EventArgs e)
    {
        throw new System.NotImplementedException();
    }

    public void CreatePlanets(ref PlanetSO[] planets, ref PlanetMeshSO planetMeshSO)
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
            planets[i].planetRef.gameObject.SetActive(false);
        }
    }
}
