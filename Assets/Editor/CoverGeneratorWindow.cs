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
        GUILayout.Space(10f);

        if (GUILayout.Button("Initialise All Patrol Points"))
        {
            foreach (PatrolManager patrolManager in FindObjectsOfType<PatrolManager>())
            {
                patrolManager.InitialiseAllZones();
            }
        }
        
        GUILayout.Space(20f);

        currentGenerator = (CoverGenerator) EditorGUILayout.ObjectField(currentGenerator, typeof(CoverGenerator), true);
        GUILayout.Label("", EditorStyles.boldLabel);


        //
        GUILayout.Space(10f);
        GUILayout.Label("Current Cover", EditorStyles.boldLabel);
        //
        // int bodiesFound = 0;
        if (GUILayout.Button("Generate Cover On Current "))
        {
            Debug.Log("RUNNING COVER GENERATION");
            if (currentGenerator)
            {
                int counterNumber= coverGeneratorController.GenerateCovers(currentGenerator);
                Debug.Log($"Generated {counterNumber} Covers");
                currentGenerator.PatrolManager.InitialiseAllZones();
            }
            else
            {
                Debug.LogWarning("Missing Current Generator");
            }
        }
        
        if (GUILayout.Button("CLEAR ALL Cover"))
        {
            if (currentGenerator)
            {
                currentGenerator.RemoveAllCover();
            }
            else
            {
                Debug.LogWarning("Missing Current Generator");
            }
        }
        //
        GUILayout.Space(10f);
        GUILayout.Label("ALL Cover", EditorStyles.boldLabel);
        //
        // int bodiesFound = 0;
        if (GUILayout.Button("Generate Cover On ALL "))
        {
            Debug.Log("RUNNING COVER GENERATION");

            foreach (CoverGenerator coverGenerator in FindObjectsOfType<CoverGenerator>())
            {
                if (coverGenerator)
                {
                    int counterNumber= coverGeneratorController.GenerateCovers(coverGenerator);
                    Debug.Log($"Generated {counterNumber} Covers for {coverGenerator.transform.parent.name}");
                    currentGenerator.PatrolManager.InitialiseAllZones();
                }
                else
                {
                    Debug.LogWarning("Missing Cover Generator");
                }
            }
            
            
        }
        
        if (GUILayout.Button("CLEAR Cover On Current "))
        {
            foreach (CoverGenerator coverGenerator in FindObjectsOfType<CoverGenerator>())
            {
                if (coverGenerator)
                {
                    coverGenerator.RemoveAllCover();
                }
                else
                {
                    Debug.LogWarning("Missing Cover Generator");
                }
            }
        }
        GUILayout.Space(10f);

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
        wallCover_Small = Resources.Load<GameObject>("Cover/WallCover_Half");
        airCover_Mid = Resources.Load<GameObject>("Cover/AirCover");
        airCover_Small = Resources.Load<GameObject>("Cover/AirCover_Small");
    }

    public CoverGeneratorController(GameObject wallCoverBig, GameObject wallCoverMid, GameObject wallCoverSmall,
        GameObject airCoverMid, GameObject airCoverSmall)
    {
        wallCover_Big = wallCoverBig;
        wallCover_Mid = wallCoverMid;
        wallCover_Small = wallCoverSmall;
        airCover_Mid = airCoverMid;
        airCover_Small = airCoverSmall;
    }

    public int GenerateCovers(CoverGenerator coverGenerator)
    {
        int i = 0;
        if (!coverGenerator.PatrolManager)
        {
            Debug.LogError("CoverGenerator missing patrolManager");
        }

        foreach (PatrolZone patrolZone in coverGenerator.PatrolManager.PatrolZones)
        {
            if (patrolZone.IsFloored)
            {
                i += GenerateFloorCovers(coverGenerator, patrolZone);
            }
            else
            {
                i += GenerateAirCovers(coverGenerator, patrolZone);
            }
        }

        
        return i;
    }

    int GenerateFloorCovers(CoverGenerator coverGenerator, PatrolZone patrolZone)
    {
        int coverCounter = 0;
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
                coverCounter++;
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

        return coverCounter;
    }

    int GenerateAirCovers(CoverGenerator coverGenerator, PatrolZone patrolZone)
    {
        int coverCounter = 0;

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
                coverCounter++;
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

        return coverCounter;
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