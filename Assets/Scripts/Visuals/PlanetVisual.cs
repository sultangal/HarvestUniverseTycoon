using UnityEngine;

public class PlanetVisual : MonoBehaviour
{
    public void SetPlanetColor(Color color)
    {
        Material mat = GetComponent<Renderer>().material;
        mat.SetColor("_Color", color);
        mat.SetColor("_FresnelColor", color*2.0f);      
    }

}
