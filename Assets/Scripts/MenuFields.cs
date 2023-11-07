using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuFields : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
        var planetsSOArr = Planets.Instance.planetsSOArr;
        for (int i = 0; i < transform.childCount; i++)
        {               
            foreach (Transform item in transform.GetChild(i))
            {
                int rnd = Random.Range(0, planetsSOArr[i].fieldItemSOs.Length);
                var instance = Instantiate(planetsSOArr[i].fieldItemSOs[rnd].fieldItemPrefab, item);
                foreach (var component in instance.GetComponents<Component>())
                {
                    if (component is not Transform && component is not FieldItemVisuals) Destroy(component);
                }
            }
        }
    }

    private void GameManager_OnGameStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameWaitingToStart())
        {
            gameObject.SetActive(true);
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            gameObject.SetActive(false);
        }
    }
}
