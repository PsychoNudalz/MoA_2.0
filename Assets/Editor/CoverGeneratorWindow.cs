#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class CoverGeneratorWindow : EditorWindow
{
    Vector2 scrollPos;
    public CoverGenerator currentGenerator;
    public CoverGenerator[] coverGenerators;
    public CoverGeneratorController coverGeneratorController;
    
    [Header("Ground Cover")]

    [SerializeField]
    private GameObject wallCover_Big;

    [SerializeField]
    private GameObject wallCover_Mid;

    [SerializeField]
    private GameObject wallCover_Small;


    [Space(10f)]
    [Header("Air Cover")]

    [SerializeField]
    private GameObject airCover_Mid;

    [SerializeField]
    private GameObject airCover_Small;

    [MenuItem("Window/Cover Generator")]
    public static void ShowWindow()
    {
        GetWindow<CoverGeneratorWindow>("Gun Effect Convertor Window");
    }

    private void OnGUI()
    {
        CoverGeneratorController coverGeneratorController = new CoverGeneratorController();
        GUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


        currentGenerator = (CoverGenerator) EditorGUILayout.ObjectField(currentGenerator, typeof(CoverGenerator), true);
        GUILayout.Label("", EditorStyles.boldLabel);


        //
        // GUILayout.Label("I have no idea what I am doing", EditorStyles.boldLabel);
        //
        // int bodiesFound = 0;
        if (GUILayout.Button("Generate Cover On Current "))
        {
            coverGeneratorController.GenerateCovers(currentGenerator);
        }
        //
        // GUILayout.Label($"Found Bodies: {bodiesFound}", EditorStyles.boldLabel);
        //
        //
        // if (GUILayout.Button("Run Conversion check"))
        // {
        // }

        GUILayout.EndScrollView();
    }


/*
    private void MarkDirty()
    {
        EditorUtility.SetDirty(cardHandler);
        EditorSceneManager.MarkSceneDirty(cardHandler.gameObject.scene);
        foreach (Card c in cardHandler.AllCards)
        {
            if (c)
            {
            EditorUtility.SetDirty(c.gameObject);
            }

        }
    }*/
}


public class CoverGeneratorController
{
    [Header("Ground Cover")]

    [SerializeField]
    private GameObject wallCover_Big;

    [SerializeField]
    private GameObject wallCover_Mid;

    [SerializeField]
    private GameObject wallCover_Small;


    [Space(10f)]
    [Header("Air Cover")]

    [SerializeField]
    private GameObject airCover_Mid;

    [SerializeField]
    private GameObject airCover_Small;
    
    public CoverGeneratorController()
    {
        wallCover_Big = Resources.Load<GameObject>("Cover/WallCover_Big");
        wallCover_Mid = Resources.Load<GameObject>("Cover/WallCover");
        wallCover_Small =Resources.Load<GameObject>("Cover/WallCover_Half");
        airCover_Mid = Resources.Load<GameObject>("Cover/AirCover");
        airCover_Small = Resources.Load<GameObject>("Cover/AirCover_Small");
    }

    public CoverGeneratorController(GameObject wallCoverBig, GameObject wallCoverMid, GameObject wallCoverSmall, GameObject airCoverMid, GameObject airCoverSmall)
    {
        wallCover_Big = wallCoverBig;
        wallCover_Mid = wallCoverMid;
        wallCover_Small = wallCoverSmall;
        airCover_Mid = airCoverMid;
        airCover_Small = airCoverSmall;
    }

    public void GenerateCovers(CoverGenerator coverGenerator)
    {
        if (!coverGenerator.PatrolManager)
        {
            Debug.LogError("CoverGenerator missing patrolManager");
        }

        foreach (PatrolZone patrolZone in coverGenerator.PatrolManager.PatrolZones)
        {
            if (patrolZone.IsFloored)
            {
                GenerateFloorCovers(coverGenerator, patrolZone);
            }
            else
            {
                GenerateAirCovers(coverGenerator, patrolZone);
            }
        }

        coverGenerator.PatrolManager.InitialiseAllZones();
    }

    void GenerateFloorCovers(CoverGenerator coverGenerator, PatrolZone patrolZone)
    {
        GameObject tempGO;
        Quaternion rotation;
        Vector3 position;
        RaycastHit hit;
        GameObject patrolParent = new GameObject($"---COVER FOR: {patrolZone.name}---");
        patrolParent.transform.SetParent(coverGenerator.transform);
        patrolParent.transform.localPosition = new Vector3();
        float randomNumber = Random.Range(0f, 1f);
        foreach (PatrolPoint patrolPoint in patrolZone.PointPositionsOpen)
        {
            randomNumber = Random.Range(0f, 1f);
            if (randomNumber < coverGenerator.GroundCoverProbability)
            {
                rotation = PickRotation();


                position = patrolPoint.Position;
                if (Physics.Raycast(position, -patrolZone.transform.up, out hit, patrolZone.PointSpacing * 2f,
                        coverGenerator.GroundLayerMask))
                {
                    position = hit.point;
                    if (randomNumber > coverGenerator.GroundCoverProbability * .85f)
                    {
                        tempGO = (GameObject) PrefabUtility.InstantiatePrefab(wallCover_Big);
                        tempGO.transform.position = position;
                        tempGO.transform.rotation = rotation;
                        tempGO.transform.SetParent(patrolParent.transform);
                    }
                    else if (randomNumber < coverGenerator.GroundCoverProbability * .5f)
                    {
                        tempGO = (GameObject) PrefabUtility.InstantiatePrefab(wallCover_Small);
                        tempGO.transform.position = position;
                        tempGO.transform.rotation = rotation;
                        tempGO.transform.SetParent(patrolParent.transform);
                    }
                    else
                    {
                        tempGO = (GameObject) PrefabUtility.InstantiatePrefab(wallCover_Mid);
                        tempGO.transform.position = position;
                        tempGO.transform.rotation = rotation;
                        tempGO.transform.SetParent(patrolParent.transform);
                    }
                }
            }
        }
    }

    void GenerateAirCovers(CoverGenerator coverGenerator,PatrolZone patrolZone)
    {
        GameObject tempGO;
        Quaternion rotation;
        Vector3 position;

        GameObject patrolParent = new GameObject($"---COVER FOR: {patrolZone.name}---");
        patrolParent.transform.SetParent(coverGenerator.transform);
        patrolParent.transform.localPosition = new Vector3();

        float randomNumber = Random.Range(0f, 1f);
        foreach (PatrolPoint patrolPoint in patrolZone.PointPositionsOpen)
        {
            randomNumber = Random.Range(0f, 1f);

            if (randomNumber < coverGenerator.AirCoverProbability)
            {
                rotation = PickRotation();
                position = patrolPoint.Position;

                if (randomNumber < coverGenerator.GroundCoverProbability * .5f)
                {
                    tempGO = (GameObject) PrefabUtility.InstantiatePrefab(airCover_Mid);
                    tempGO.transform.position = position;
                    tempGO.transform.rotation = rotation;
                    tempGO.transform.SetParent(patrolParent.transform);
                }
                else
                {
                    tempGO = (GameObject) PrefabUtility.InstantiatePrefab(airCover_Small);
                    tempGO.transform.position = position;
                    tempGO.transform.rotation = rotation;
                    tempGO.transform.SetParent(patrolParent.transform);
                }
            }
        }
    }

    private Quaternion PickRotation()
    {
        Quaternion rotation;
        if (Random.Range(0, 1f) > .75f)
        {
            rotation = Quaternion.Euler(0f, 270f, 0f);
        }
        else if (Random.Range(0, 1f) > .5f)
        {
            rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else if (Random.Range(0, 1f) > .25f)
        {
            rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else
        {
            rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        return rotation;
    }
}

public class CoverGeneratorWindowInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUI.changed)
        {
        }
    }
}
#endif