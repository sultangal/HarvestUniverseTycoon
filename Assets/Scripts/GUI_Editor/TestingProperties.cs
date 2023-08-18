using UnityEditor;
using UnityEngine;
using System.Collections;
using static GameManager;
using static UnityEngine.GraphicsBuffer;

public class TestingProperties : EditorWindow
{
    private GameState testState;

    private float speedMult = 1f;
    private float bladesWidthMult = 1f;
    private HarvesterGroup harvesterGroupRef;

    public float respawnPointRemoteness = 30f;
    public float asteroidMoveSpeed = 9f;
    public int minSecBetweenRespawn = 0;
    public int maxSecBetweenRespawn = 1;
    private Asteroids asteroidsRef;

    
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
        harvesterGroupRef = FindFirstObjectByType<HarvesterGroup>();
        speedMult = EditorGUILayout.Slider("Speed mult:", speedMult, 1f, 1.5f);
        harvesterGroupRef.harvesterSpeedMult = speedMult;
        bladesWidthMult = EditorGUILayout.Slider("Blades mult:", bladesWidthMult, 1f, 4.5f);       
        harvesterGroupRef.bladesWidthMult = bladesWidthMult;

        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("Asteroid settings: ", EditorStyles.boldLabel);
        asteroidsRef = FindFirstObjectByType<Asteroids>();
        respawnPointRemoteness = EditorGUILayout.FloatField("Respawn point remoteness:", respawnPointRemoteness);
        asteroidsRef.respawnPointRemoteness = respawnPointRemoteness;

        asteroidMoveSpeed = EditorGUILayout.FloatField("Move speed:", asteroidMoveSpeed);
        asteroidsRef.asteroidMoveSpeed = asteroidMoveSpeed;

        EditorGUILayout.BeginHorizontal();
        minSecBetweenRespawn = EditorGUILayout.IntField("Min sec between respawn:", minSecBetweenRespawn);
        asteroidsRef.minSecBetweenRespawn = minSecBetweenRespawn;
        EditorGUILayout.Space(5);
        maxSecBetweenRespawn = EditorGUILayout.IntField("Max sec between respawn:", maxSecBetweenRespawn);
        asteroidsRef.maxSecBetweenRespawn = maxSecBetweenRespawn;
        EditorGUILayout.EndHorizontal();
    }

    private void Update()
    {
        //TODO: Make GameSessionData showing on window
    }
}