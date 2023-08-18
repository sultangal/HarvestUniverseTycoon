using UnityEditor;
using UnityEngine;
using System.Collections;
using static GameManager;
using static UnityEngine.GraphicsBuffer;

public class TestingProperties : EditorWindow
{
    private float speedMult = 1f;
    private float bladesWidthMult = 1f;
    private GameObject harvesterGroupRef;
    private GameState testState;
    [MenuItem("Testing/Test Window")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(TestingProperties));
        window.Show();
    }

    private void OnGUI()
    {

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Game state settings: ", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        testState = (GameState)EditorGUILayout.EnumPopup("Set game tate: ", testState);
        if (GUILayout.Button("SET"))
        {
            GameManager.Instance.SetGameState(testState);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Harvester settings: ", EditorStyles.boldLabel);

        harvesterGroupRef = (GameObject)EditorGUILayout.ObjectField("Harvester group ref: ",
            harvesterGroupRef, typeof(GameObject), true);
        speedMult = EditorGUILayout.Slider("Speed mult:", speedMult, 1f, 1.5f);
        bladesWidthMult = EditorGUILayout.Slider("Blades mult:", bladesWidthMult, 1f, 4.5f);

        if (GUILayout.Button("SET"))
        {
            harvesterGroupRef.GetComponent<HarvesterGroup>().harvesterSpeedMult = speedMult;
            harvesterGroupRef.GetComponent<HarvesterGroup>().bladesWidthMult = bladesWidthMult;
        }
    }

}