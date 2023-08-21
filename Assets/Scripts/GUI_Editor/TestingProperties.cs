using UnityEditor;
using UnityEngine;
using static GameManager;

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

    private int collectedCash = 0;
    private int collectedGold = 0;
    private FieldItemSO[] fieldItemSOs = null;
    public int[] collectedFieldItemSOs;
    private Vector3 curentPlanetPosition = Vector3.zero;


    [MenuItem("Testing/Test Window")]
    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(TestingProperties));
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(20);
        GuiLine();
        EditorGUILayout.LabelField("Game state settings: ", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        testState = (GameState)EditorGUILayout.EnumPopup("Set game tate: ", testState);
        if (GUILayout.Button("SET"))
        {
            GameManager.Instance.SetGameState(testState);
            OnGUI();
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

        EditorGUILayout.Space(20);
        GuiLine();

        if (EditorApplication.isPlaying)
        {
            EditorGUILayout.LabelField("Game session data: ", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Collected Cash: " + collectedCash.ToString());
            EditorGUILayout.LabelField("Collected Gold: " + collectedGold.ToString());
            EditorGUILayout.LabelField("fieldItemSOs: ");
            if (fieldItemSOs != null && collectedFieldItemSOs != null && fieldItemSOs.Length == collectedFieldItemSOs.Length)
            {
                for (var i = 0; i < fieldItemSOs.Length; i++)
                {
                    EditorGUILayout.LabelField("    " + fieldItemSOs[i].ToString() + " collected: " + collectedFieldItemSOs[i]);
                }

            }
            EditorGUILayout.LabelField("Current planet position: " + curentPlanetPosition.ToString());
            EditorGUILayout.Space(20);
            GuiLine();
        }
    }

    void GuiLine(int height = 1)

    {

        Rect rect = EditorGUILayout.GetControlRect(false, height);

        rect.height = height;

        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));

    }

    private void OnInspectorUpdate()
    {
        if (EditorApplication.isPlaying && GameManager.Instance != null)
        {
            collectedCash = GameManager.Instance.GameSessionData.collectedCash;
            collectedGold = GameManager.Instance.GameSessionData.collectedGold;
            fieldItemSOs = GameManager.Instance.GameSessionData.FieldItemSOs;
            collectedFieldItemSOs = GameManager.Instance.GameSessionData.CollectedFieldItemSOs;
            curentPlanetPosition = GameManager.Instance.GameSessionData.CurentPlanetPosition;
        }
    }
}