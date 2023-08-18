using UnityEditor;
using static GameManager;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private GameState testState;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("'PROPERTIES FOR TESTING PURPOSE: '", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
  
        serializedObject.UpdateIfRequiredOrScript();
        GameManager gm = (GameManager)target;
        testState = (GameState)EditorGUILayout.EnumPopup("Set Game State: ", testState);
        if (GUILayout.Button("SET"))
        {
            gm.SetGameState(testState);
        }
        
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("'", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);
    }
}
