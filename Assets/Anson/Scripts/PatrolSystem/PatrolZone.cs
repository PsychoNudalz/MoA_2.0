using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;
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
    private HashSet<Vector3> pointPositions;

    [SerializeField]
    private List<Vector3> pointPositions_Cover;

    [SerializeField]
    private List<Vector3> pointPositions_Open;

    public HashSet<Vector3> PointPositions => pointPositions;

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

    [Space(10f)]
    [SerializeField]
    private bool debug_ShowGetPosition;


    [SerializeField]
    private Transform debug_GetPositionTransform;

    [Space(10f)]
    [SerializeField]
    private bool debug_ShowTempList;

    [SerializeField]
    private List<Vector3> debug_TempList;

    private Vector3 startingPoint;

    [Space(10f)]
    [SerializeField]
    private bool debug_TestTime;


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


            if (pointPositions != null)
            {
                foreach (Vector3 pointPosition in pointPositions)

                {
                    if (pointPositions_Cover.Contains(pointPosition))
                    {
                        Gizmos.color = Color.cyan- new Color(0, 0, 0, .2f);
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

            if (debug_ShowGetPosition)
            {
                if (debug_GetPositionTransform)
                {
                    List<Vector3> temp = GetPoints(debug_GetPositionTransform.position,
                        debug_GetPositionTransform.localScale.x / 2f);
                    foreach (Vector3 getPoint in temp)
                    {
                        Gizmos.color = Color.green - new Color(0, 0, 0, .5f);
                        Gizmos.DrawCube(getPoint,
                            new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f));
                    }
                }
            }


            if (debug_ShowTempList)
            {
                foreach (Vector3 tempPoint in debug_TempList)
                {
                    Gizmos.color = Color.red - new Color(0, 0, 0, .5f);
                    Gizmos.DrawCube(tempPoint,
                        new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f));
                }
            }
        }

        if (debug_TestTime)
        {
            float startTime = Time.realtimeSinceStartup;
            List<Vector3> temp = GetPoints(debug_GetPositionTransform.position,
                debug_GetPositionTransform.localScale.x / 2f);
            if (temp.Count > 0)
            {
                Vector3 pointPosition = temp[Random.Range(0, temp.Count)];
            }

            print($"Time:{Time.realtimeSinceStartup - startTime}.  Count: {temp.Count}");
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
        pointPositions = new HashSet<Vector3>();
        var boxLocalScale = boxZone.localScale;
        startingPoint = boxZone.position;
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
                        pointPositions.Add(ConvertPoint(hit.point + new Vector3(0, pointSpacing / 2f, 0), false));
                        // pointPositions.Add(hit.point + new Vector3(0, pointSpacing / 2f, 0));
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
        startingPoint -= new Vector3(boxLocalScale.x / 2, boxLocalScale.y / 2, boxLocalScale.z / 2);
        foreach (Vector3 newPoint in ScanSurrounding(startingPoint, boxLocalScale.x, boxLocalScale.y, boxLocalScale.z))
        {
            pointPositions.Add(newPoint);
        }
    }

    private List<Vector3> ScanSurrounding(Vector3 startingPoint, float xSize, float ySize, float zSize)
    {
        Vector3 newPoint;
        List<Vector3> tempList = new List<Vector3>();

        for (int x = 0; x < Mathf.FloorToInt(xSize) / pointSpacing; x++)
        {
            for (int y = 0; y < Mathf.FloorToInt(ySize) / pointSpacing; y++)
            {
                for (int z = 0; z < Mathf.FloorToInt(zSize) / pointSpacing; z++)
                {
                    newPoint = startingPoint + new Vector3(x * pointSpacing, y * pointSpacing, z * pointSpacing);
                    // pointPositions.Add(newPoint);
                    tempList.Add(newPoint);
                    // if (showDebug)
                    // {
                    //     print($"point index {x}, {y},{z}: {newPoint}");
                    // }
                }
            }
        }

        return tempList;
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
        return pointPositions.ElementAt(Random.Range(0, pointPositions.Count));
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

    public List<Vector3> GetPoints(Vector3 position, float range)
    {
        List<Vector3> finalPoints = new List<Vector3>();

        position = ConvertPoint(position);

        // finalPoints.Add(position);
        if (debug_ShowTempList)
        {
            debug_TempList = new List<Vector3>();
        }

        int count = 0;
        Vector3 offset = new Vector3(range - pointSpacing / 2f, range - pointSpacing / 2f, range - pointSpacing / 2f);
        foreach (Vector3 p in ScanSurrounding(position - offset, range * 2, range * 2,
                     range * 2))
        {
            if (pointPositions.Contains(p))
            {
                finalPoints.Add(p);
            }

            if (debug_ShowTempList)
            {
                debug_TempList.Add(p);
            }

            count++;
        }

        // print(count);


        return finalPoints;
    }

    public Vector3 ConvertPoint(Vector3 point, bool halfSpacing = true)
    {
        point /= pointSpacing/2f;
        Vector3 temp = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));

        if (useWorldPositions)
        {
            if (halfSpacing)
            {
                temp += new Vector3(pointSpacing / 2f, 0, pointSpacing / 2f);
            }
        }

        temp *= pointSpacing/2f;
        if (!useWorldPositions)
        {
            Vector3 offset = startingPoint - new Vector3(Mathf.Round(startingPoint.x),
                Mathf.Round(startingPoint.y) + pointSpacing / 2f, Mathf.Round(startingPoint.z));
            temp += offset;
        }


        return temp;
    }
}