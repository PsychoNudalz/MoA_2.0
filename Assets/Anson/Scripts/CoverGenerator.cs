using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverGenerator : MonoBehaviour
{
    [Header("Ground Cover")]
    [SerializeField]
    [Range(0f, 1f)]
    private float groundCoverProbability = 0.1f;

    [SerializeField]
    private GameObject wallCover_Big;

    [SerializeField]
    private GameObject wallCover_Mid;

    [SerializeField]
    private GameObject wallCover_Small;

    [SerializeField]
    private LayerMask groundLayerMask;

    [Space(10f)]
    [Header("Air Cover")]
    [SerializeField]
    [Range(0f, 1f)]
    private float airCoverProbability = 0.1f;

    [SerializeField]
    private GameObject airCover_Mid;

    [SerializeField]
    private GameObject airCover_Small;

    [Space(10f)]
    [SerializeField]
    private PatrolManager patrolManager;


    public float GroundCoverProbability => groundCoverProbability;

    public LayerMask GroundLayerMask => groundLayerMask;

    public float AirCoverProbability => airCoverProbability;

    public PatrolManager PatrolManager => patrolManager;


    [ContextMenu("Remove All Covers")]
    public void RemoveAllCover()
    {
        Transform[] temp = GetComponentsInChildren<Transform>();
        foreach (Transform t in temp)
        {
            if (!t.Equals(transform))
            {
                if (t)
                {
                    DestroyImmediate(t.gameObject);
                }
            }
        }
    }

    [ContextMenu("Generate Covers")]
    public void GenerateCovers()
    {
        if (!patrolManager)
        {
            Debug.LogError("CoverGenerator missing patrolManager");
        }

        foreach (PatrolZone patrolZone in patrolManager.PatrolZones)
        {
            if (patrolZone.IsFloored)
            {
                GenerateFloorCovers(patrolZone);
            }
            else
            {
                GenerateAirCovers(patrolZone);
            }
        }

        patrolManager.InitialiseAllZones();
    }

    void GenerateFloorCovers(PatrolZone patrolZone)
    {
        GameObject tempGO;
        Quaternion rotation;
        Vector3 position;
        RaycastHit hit;
        GameObject patrolParent = new GameObject($"---COVER FOR: {patrolZone.name}---");
        patrolParent.transform.SetParent(transform);
        patrolParent.transform.localPosition = new Vector3();
        float randomNumber = Random.Range(0f, 1f);
        foreach (PatrolPoint patrolPoint in patrolZone.PointPositionsOpen)
        {
            randomNumber = Random.Range(0f, 1f);
            if (randomNumber < groundCoverProbability)
            {
                rotation = PickRotation();


                position = patrolPoint.Position;
                if (Physics.Raycast(position, -patrolZone.transform.up,out hit, patrolZone.PointSpacing * 2f, groundLayerMask))
                {
                    position = hit.point;
                    if (randomNumber > groundCoverProbability * .85f)
                    {
                        tempGO = Instantiate(wallCover_Big, position, rotation, patrolParent.transform);
                    }
                    else if (randomNumber < groundCoverProbability * .5f)
                    {
                        tempGO = Instantiate(wallCover_Small, position, rotation, patrolParent.transform);
                    }
                    else
                    {
                        tempGO = Instantiate(wallCover_Mid, position, rotation, patrolParent.transform);
                    }
                }
                
            }
        }
    }

    void GenerateAirCovers(PatrolZone patrolZone)
    {
        GameObject tempGO;
        Quaternion rotation;
        GameObject patrolParent = new GameObject($"---COVER FOR: {patrolZone.name}---");
        patrolParent.transform.SetParent(transform);
        patrolParent.transform.localPosition = new Vector3();

        float randomNumber = Random.Range(0f, 1f);
        foreach (PatrolPoint patrolPoint in patrolZone.PointPositionsOpen)
        {
            randomNumber = Random.Range(0f, 1f);

            if (randomNumber < airCoverProbability)
            {
                rotation = PickRotation();

                if (randomNumber > airCoverProbability * .5f)
                {
                    tempGO = Instantiate(airCover_Mid, patrolPoint.Position, rotation, patrolParent.transform);
                }
                else
                {
                    tempGO = Instantiate(airCover_Small, patrolPoint.Position, rotation, patrolParent.transform);
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