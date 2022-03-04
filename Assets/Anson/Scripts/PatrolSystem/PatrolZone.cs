using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrolZone : MonoBehaviour
{
    [SerializeField]
    private Transform boxZone;

    [SerializeField]
    private List<Collider> detectedColliders;

    [Header("Settings")]
    [SerializeField]
    private LayerMask floorLayerMask;

    [SerializeField]
    private List<string> floorTags;

    [SerializeField]
    private LayerMask invalidLayerMask;

    [SerializeField]
    private List<string> invalidTags;

    [SerializeField]
    private LayerMask coverLayerMask;

    [SerializeField]
    private List<string> coverTags;


    [Space(15f)]
    [SerializeField]
    private bool isFloored;

    [SerializeField]
    private bool useVolume;

    [SerializeField]
    private bool useWorldPositions;

    [SerializeField]
    private float pointSpacing = 0.5f;

    private float oldSpacing = .5f;

    [SerializeField]
    private float invalidPointSpacing = 0.3f;

    private float oldInvalidSpacing = .5f;

    [SerializeField]
    private float coverSpacing = 0.5f;

    private float oldCoverSpacing = .5f;

    [Space(20f)]
    [Header("Points")]
    [SerializeField]
    private List<Vector3> pointPositions;

    [SerializeField]
    private List<Vector3> pointPositions_Cover;

    [SerializeField]
    private List<Vector3> pointPositions_Open;

    public List<Vector3> PointPositions => pointPositions;

    public List<Vector3> PointPositionsCover => pointPositions_Cover;

    public List<Vector3> PointPositionsOpen => pointPositions_Open;

    [Space(10f)]
    [Header("Debug")]
    [SerializeField]
    private bool showDebug;

    [SerializeField]
    private bool debug_showCover;

    [SerializeField]
    private bool debug_autoRegenerate;

    private Vector3 lastStartingPoint;

    [SerializeField]
    private bool debug_showSpacing;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        if (showDebug)
        {
            if (debug_autoRegenerate)
            {
                var boxLocalScale = boxZone.localScale;

                Vector3 currentStartingPoint = boxZone.position -
                                               new Vector3(boxLocalScale.x / 2, boxLocalScale.y / 2,
                                                   boxLocalScale.z / 2);
                if (IsChanged(currentStartingPoint))
                {
                    print("Running Auto Generator");
                    GenerateAll();
                }
            }

            foreach (Vector3 pointPosition in pointPositions)

            {
                if (pointPositions_Cover.Contains(pointPosition))
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawCube(pointPosition,
                        new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f));
                }
                else
                {
                    Gizmos.color = Color.grey;
                    Gizmos.DrawCube(pointPosition,
                        new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f));
                }

                if (debug_showSpacing)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(pointPosition, pointSpacing);
                }
            }
        }
    }

    bool IsChanged(Vector3 currentStartingPoint)
    {
        bool samePoint = !currentStartingPoint.Equals(lastStartingPoint);
        bool temp1 = Math.Abs(oldSpacing - pointSpacing) < .01f;
        bool temp2 = Math.Abs(oldInvalidSpacing - invalidPointSpacing) < .01f;
        bool temp3 = Math.Abs(oldCoverSpacing - coverSpacing) < .01f;

        lastStartingPoint = currentStartingPoint;
        oldSpacing = pointSpacing;
        oldInvalidSpacing = invalidPointSpacing;
        oldCoverSpacing = coverSpacing;
        return samePoint || !temp1 || !temp2 || !temp3;
    }

    [ContextMenu("Generate all")]
    public void GenerateAll()
    {
        GeneratePoints();
        SortPoints();
    }


    [ContextMenu("Generate Points")]
    public void GeneratePoints()
    {
        pointPositions = new List<Vector3>();
        var boxLocalScale = boxZone.localScale;
        Vector3 startingPoint = boxZone.position;
        if (useWorldPositions)
        {
            startingPoint = new Vector3(Mathf.RoundToInt(startingPoint.x), Mathf.RoundToInt(startingPoint.y),
                Mathf.RoundToInt(startingPoint.z));
        }

        startingPoint += new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f);

        if (isFloored)
        {
            GeneratePoints_Floor(startingPoint, boxLocalScale);
        }
        else
        {
            GeneratePoints_Box(startingPoint, boxLocalScale);
        }

        RemoveInvalidPoints();
    }

    private void GeneratePoints_Floor(Vector3 startingPoint, Vector3 boxLocalScale)
    {
        Vector3 newPoint;
        Vector3 downVector;
        RaycastHit hit;


        if (useWorldPositions)
        {
            downVector = Vector3.down;
        }
        else
        {
            downVector = -boxZone.up;
        }

        startingPoint -= new Vector3(boxLocalScale.x / 2, -boxLocalScale.y / 2, boxLocalScale.z / 2);
        for (int x = 0; x < Mathf.FloorToInt(boxLocalScale.x) / pointSpacing; x++)
        {
            for (int z = 0; z < Mathf.FloorToInt(boxLocalScale.z) / pointSpacing; z++)
            {
                if (Physics.Raycast((startingPoint + new Vector3(x * pointSpacing, 0, z * pointSpacing)), downVector,
                    out hit, boxLocalScale.y + pointSpacing, floorLayerMask))
                {
                    if (floorTags.Contains(hit.collider.tag))
                    {
                        pointPositions.Add(hit.point + new Vector3(0, pointSpacing / 2f, 0));
                    }
                }

                // if (showDebug)
                // {
                //     print($"point index {x}, {y},{z}: {newPoint}");
                // }
            }
        }
    }

    private void GeneratePoints_Box(Vector3 startingPoint, Vector3 boxLocalScale)
    {
        Vector3 newPoint;

        startingPoint -= new Vector3(boxLocalScale.x / 2, boxLocalScale.y / 2, boxLocalScale.z / 2);
        for (int x = 0; x < Mathf.FloorToInt(boxLocalScale.x) / pointSpacing; x++)
        {
            for (int y = 0; y < Mathf.FloorToInt(boxLocalScale.y) / pointSpacing; y++)
            {
                for (int z = 0; z < Mathf.FloorToInt(boxLocalScale.z) / pointSpacing; z++)
                {
                    newPoint = startingPoint + new Vector3(x * pointSpacing, y * pointSpacing, z * pointSpacing);
                    pointPositions.Add(newPoint);
                    // if (showDebug)
                    // {
                    //     print($"point index {x}, {y},{z}: {newPoint}");
                    // }
                }
            }
        }
    }

    public void RemoveInvalidPoints()
    {
        DetectCollidersInZone();
        List<Vector3> pointsToRemove = new List<Vector3>();
        RaycastHit[] hits;
        foreach (Collider detectedCollider in detectedColliders)
        {
            pointsToRemove = new List<Vector3>();
            foreach (Vector3 pointPosition in pointPositions)
            {
                if (detectedCollider.bounds.Contains(pointPosition))
                {
                    pointsToRemove.Add(pointPosition);
                }
                else
                {
                    hits =
                        Physics.SphereCastAll(pointPosition, invalidPointSpacing, Vector3.down, invalidPointSpacing,
                            invalidLayerMask);

                    foreach (RaycastHit raycastHit in hits)
                    {
                        if (invalidTags.Contains(raycastHit.collider.tag))
                        {
                            pointsToRemove.Add(pointPosition);
                            break;
                        }
                    }
                }
            }

            foreach (Vector3 vector3 in pointsToRemove)
            {
                pointPositions.Remove(vector3);
            }
        }
    }

    private void DetectCollidersInZone()
    {
        detectedColliders = new List<Collider>();
        RaycastHit[] allHits =
            Physics.BoxCastAll(boxZone.position, boxZone.lossyScale, Vector3.up, quaternion.identity);
        foreach (RaycastHit hit in allHits)
        {
            detectedColliders.Add(hit.collider);
        }
    }


    public Vector3 GetRandomPoint()
    {
        return pointPositions[Random.Range(0, pointPositions.Count)];
    }

    [ContextMenu("Sort Points")]
    public void SortPoints()
    {
        bool flag = false;
        int i;

        pointPositions_Cover = new List<Vector3>();
        pointPositions_Open = new List<Vector3>();
        foreach (Vector3 pointPosition in pointPositions)
        {
            flag = false;
            i = 0;

            RaycastHit[] hits =
                Physics.SphereCastAll(pointPosition, coverSpacing, Vector3.down, coverSpacing, coverLayerMask);

            while (i < hits.Length && !flag)
            {
                if (coverTags.Contains(hits[i].collider.tag))
                {
                    pointPositions_Cover.Add(pointPosition);
                    flag = true;
                }

                i++;
            }

            if (!flag)
            {
                pointPositions_Open.Add(pointPosition);
            }
        }
    }
}