using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static GameManager;

public class TestingProperties : EditorWindow
{
    private GameState testState;

    private float speedMult = 1f;
    private float bladesWidthMult = 1f;
    private HarvesterGroup harvesterGroupRef;
    private GameManager gm;
    private float countdown = 59f;

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

    public List<GameObject> cratersList;
    public List<GameObject> asteroidsList;
    public List<GameObject> goldsList;

    public int pointsQuantity = 0;
    public int amountOfGold = 0;
    public int amountOfCash = 0;
    public int level = 0;

    public FieldItemSO[] fieldItemsOnLevel;
    public int[] amountOfCollectedFieldItemsOnLevel;
    public bool[] goalAchievedFlags;


    [MenuItem("Testing/Testing Properties")]
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
        testState = (GameState)EditorGUILayout.EnumPopup("    Set game tate: ", testState);
        if (GUILayout.Button("SET"))
        {
            GameManager.Instance.SetGameState(testState);
            OnGUI();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);
        GuiLine();

        EditorGUILayout.LabelField("Harvester settings: ", EditorStyles.boldLabel);
        harvesterGroupRef = FindFirstObjectByType<HarvesterGroup>();
        speedMult = EditorGUILayout.Slider("    Speed mult:", speedMult, 1f, 1.5f);
        harvesterGroupRef.harvesterSpeedMult = speedMult;
        bladesWidthMult = EditorGUILayout.Slider("    Blades mult:", bladesWidthMult, 1f, 4.5f);
        harvesterGroupRef.bladesWidthMult = bladesWidthMult;

        EditorGUILayout.Space(20);
        GuiLine();

        EditorGUIUtility.labelWidth = 200;
        EditorGUILayout.LabelField("Asteroid settings: ", EditorStyles.boldLabel);
        asteroidsRef = FindFirstObjectByType<Asteroids>();
        respawnPointRemoteness = EditorGUILayout.FloatField("    Respawn point remoteness:", respawnPointRemoteness);
        asteroidsRef.respawnPointRemoteness = respawnPointRemoteness;
        asteroidMoveSpeed = EditorGUILayout.FloatField("    Move speed:", asteroidMoveSpeed);
        asteroidsRef.asteroidMoveSpeed = asteroidMoveSpeed;      
        minSecBetweenRespawn = EditorGUILayout.IntField("    Min sec between respawn:", minSecBetweenRespawn);
        asteroidsRef.minSecBetweenRespawn = minSecBetweenRespawn;
        maxSecBetweenRespawn = EditorGUILayout.IntField("    Max sec between respawn:", maxSecBetweenRespawn);
        asteroidsRef.maxSecBetweenRespawn = maxSecBetweenRespawn;

        EditorGUILayout.Space(20);
        GuiLine();

        EditorGUILayout.LabelField("Global data: ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("    Points quantity: " + pointsQuantity.ToString());
        EditorGUILayout.LabelField("    Amount of gold: " + amountOfGold.ToString());
        EditorGUILayout.LabelField("    Amount of cash: " + amountOfCash.ToString());
        EditorGUILayout.LabelField("    level: " + level.ToString());

        EditorGUILayout.Space(20);
        GuiLine();


        EditorGUILayout.LabelField("Level data: ", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("    fieldItemSOs: ");
        if (fieldItemsOnLevel != null)
        {
            for (var i = 0; i < fieldItemsOnLevel.Length; i++)
            {
                EditorGUILayout.LabelField("        " + fieldItemsOnLevel[i].ToString() +
                    "   collected on level: "
                    + amountOfCollectedFieldItemsOnLevel[i] +
                    "   flags: " +
                    goalAchievedFlags[i]);
            }
        }

        EditorGUILayout.Space(20);
        GuiLine();

        if (EditorApplication.isPlaying)
        {
            EditorGUILayout.LabelField("Game session data: ", EditorStyles.boldLabel);
            gm = FindFirstObjectByType<GameManager>();
            countdown = EditorGUILayout.FloatField("    Countdown:", countdown);
            gm.COUNTDOWN_TIME = countdown;
            EditorGUILayout.LabelField("    Collected Cash: " + collectedCash.ToString());
            EditorGUILayout.LabelField("    Collected Gold: " + collectedGold.ToString());
            EditorGUILayout.LabelField("    fieldItemSOs: ");
            if (fieldItemSOs != null && collectedFieldItemSOs != null && fieldItemSOs.Length == collectedFieldItemSOs.Length)
            {
                for (var i = 0; i < fieldItemSOs.Length; i++)
                {
                    EditorGUILayout.LabelField("        " + fieldItemSOs[i].ToString() + " collected in session: " + collectedFieldItemSOs[i]);
                }

            }
            EditorGUILayout.LabelField("    Current planet position: " + curentPlanetPosition.ToString());
            EditorGUILayout.Space(20);

            EditorGUILayout.LabelField("    Craters list Count: " + cratersList.Count.ToString());
            EditorGUILayout.LabelField("    Asteroids list Count: " + asteroidsList.Count.ToString());
            EditorGUILayout.LabelField("    Golds list Count: " + goldsList.Count.ToString());
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
            collectedCash = GameManager.Instance.GameSessionData_.collectedCash;
            collectedGold = GameManager.Instance.GameSessionData_.collectedGold;
            fieldItemSOs = GameManager.Instance.GameSessionData_.FieldItemsOnLevel;
            collectedFieldItemSOs = GameManager.Instance.GameSessionData_.CollectedFieldItems;
            curentPlanetPosition = GameManager.Instance.GameSessionData_.CurentPlanetPosition;

            cratersList = Asteroids.Instance.CratersList;
            asteroidsList = Asteroids.Instance.AsteroidsList;
            goldsList = Asteroids.Instance.GoldsList;

            pointsQuantity = GameManager.Instance.GlobalData_.pointsQuantity;
            amountOfGold = GameManager.Instance.GlobalData_.amountOfGold;
            amountOfCash = GameManager.Instance.GlobalData_.amountOfCash;
            level = GameManager.Instance.GlobalData_.level;

            fieldItemsOnLevel = GameManager.Instance.LevelData_.FieldItemsOnLevel;
            amountOfCollectedFieldItemsOnLevel = GameManager.Instance.LevelData_.AmountOfCollectedFieldItemsOnLevel;
            goalAchievedFlags = GameManager.Instance.LevelData_.GoalAchievedFlags;
        }
    }
}