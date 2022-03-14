using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public class PatrolPoint
{
    [SerializeField]
    private Vector3 position;

    [SerializeField]
    private bool isCover;

    [SerializeField]
    private Vector3 coverDirection;

    public Vector3 Position
    {
        get => position;
        set => position = value;
    }


    public bool IsCover
    {
        get => isCover;
        set => isCover = value;
    }

    public Vector3 CoverDirection
    {
        get => coverDirection;
        set => coverDirection = value;
    }

    public PatrolPoint(Vector3 position, bool isCover, Vector3 coverDirection)
    {
        this.position = position;
        this.isCover = isCover;
        this.coverDirection = coverDirection;
    }

    public PatrolPoint(Vector3 position)
    {
        this.position = position;
        isCover = false;
        coverDirection = new Vector3();
    }


    public override bool Equals(object obj)
    {
// If the passed object is null
        if (obj == null)
        {
            return false;
        }

        if (!(obj is PatrolPoint))
        {
            return false;
        }

        return (this.position == ((PatrolPoint) obj).position);
    }

    public override int GetHashCode()
    {
        return position.GetHashCode();
    }
}

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
    private HashSet<PatrolPoint> pointPositions;

    [SerializeField]
    private List<PatrolPoint> pointPositions_Cover;

    [SerializeField]
    private List<PatrolPoint> pointPositions_Open;

    public HashSet<PatrolPoint> PointPositions => pointPositions;

    public List<PatrolPoint> PointPositionsCover => pointPositions_Cover;

    public List<PatrolPoint> PointPositionsOpen => pointPositions_Open;

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

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            double CRASHTHRESSHOLD = .1d;
            if (pointSpacing >= CRASHTHRESSHOLD && debug_autoRegenerate)
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
                foreach (PatrolPoint pointPosition in pointPositions)

                {
                    if (pointPositions_Cover.Contains(pointPosition))
                    {
                        Gizmos.color = Color.cyan - new Color(0, 0, 0, .2f);
                        Gizmos.DrawCube(pointPosition.Position,
                            new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f));
                    }
                    else
                    {
                        Gizmos.color = Color.grey;
                        Gizmos.DrawCube(pointPosition.Position,
                            new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f));
                    }

                    if (debug_showSpacing)
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(pointPosition.Position, pointSpacing);
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

            if (pointSpacing >= CRASHTHRESSHOLD && debug_autoRegenerate && debug_ShowGetPosition)
            {
                if (debug_GetPositionTransform && pointPositions != null)
                {
                    List<PatrolPoint> getPointsReturn = GetPoints(debug_GetPositionTransform.position,
                        debug_GetPositionTransform.localScale.x / 2f);
                    foreach (PatrolPoint getPoint in getPointsReturn)
                    {
                        Gizmos.color = Color.green - new Color(0, 0, 0, .5f);
                        Gizmos.DrawCube(getPoint.Position,
                            new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f));
                    }
                }
            }

            if (pointPositions != null && pointSpacing >= CRASHTHRESSHOLD && debug_TestTime)
            {
                float startTime = Time.realtimeSinceStartup;
                List<PatrolPoint> temp = GetPoints(debug_GetPositionTransform.position,
                    debug_GetPositionTransform.localScale.x / 2f);
                if (temp.Count > 0)
                {
                    PatrolPoint pointPosition = temp[Random.Range(0, temp.Count)];
                }

                print($"Time:{Time.realtimeSinceStartup - startTime}.  Count: {temp.Count}");
            }
        }

        // if (pointPositions != null)
        // {
        //     HashSet<PatrolPoint>.Enumerator e = pointPositions.GetEnumerator();
        //     e.MoveNext();
        //     Vector3 FirstValueEqual = e.Current.Position;
        //     PatrolPoint TestFirstValueEqual2 = new PatrolPoint( new Vector3(FirstValueEqual.x,
        //         FirstValueEqual.y, FirstValueEqual.z));
        //     print(e.Current.Equals(TestFirstValueEqual2));
        // }
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
        pointPositions = new HashSet<PatrolPoint>();
        var boxLocalScale = boxZone.localScale;
        startingPoint = boxZone.position;
        if (useWorldPositions)
        {
            startingPoint = ConvertPoint(startingPoint);
        }

        // startingPoint += new Vector3(pointSpacing / 2f, pointSpacing / 2f, pointSpacing / 2f);

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
                        pointPositions.Add(
                            new PatrolPoint(ConvertPoint(hit.point)));
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
        // startingPoint -= new Vector3(boxLocalScale.x / 2f, boxLocalScale.y / 2f, boxLocalScale.z / 2f);
        foreach (Vector3 newPoint in ScanSurrounding(startingPoint, boxLocalScale.x, boxLocalScale.y, boxLocalScale.z))
        {
            pointPositions.Add(new PatrolPoint(newPoint));
        }
    }

    private List<Vector3> ScanSurrounding(Vector3 centrePoint, float xSize, float ySize, float zSize,
        bool isShpere = false, Vector3 centre = new Vector3(), float pointSpacing = 0)
    {
        if (pointSpacing == 0)
        {
            pointSpacing = this.pointSpacing;
        }

        Vector3 newPoint;
        Vector3 temp;
        List<Vector3> tempList = new List<Vector3>();

        for (int x = 0; x <= xSize / pointSpacing; x++)
        {
            for (int y = 0; y <= ySize / pointSpacing; y++)
            {
                for (int z = 0; z <= zSize / pointSpacing; z++)
                {
                    newPoint = centrePoint +
                               new Vector3(((int) x / 2) * pointSpacing * IsEvenFlip(x),
                                   ((int) y / 2) * pointSpacing * IsEvenFlip(y),
                                   ((int) z / 2) * pointSpacing * IsEvenFlip(z));
                    if (isShpere)
                    {
                        if (Vector3.Distance(newPoint, centre) < xSize / 2f)
                        {
                            // pointPositions.Add(newPoint);
                            tempList.Add(newPoint);
                        }
                    }
                    else
                    {
                        // pointPositions.Add(newPoint);
                        tempList.Add(newPoint);
                    }

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
        List<PatrolPoint> pointsToRemove = new List<PatrolPoint>();
        RaycastHit[] hits;
        foreach (Collider detectedCollider in detectedColliders)
        {
            pointsToRemove = new List<PatrolPoint>();
            foreach (PatrolPoint pointPosition in pointPositions)
            {
                if (detectedCollider.bounds.Contains(pointPosition.Position))
                {
                    pointsToRemove.Add(pointPosition);
                }
                else
                {
                    hits =
                        Physics.SphereCastAll(pointPosition.Position, invalidPointSpacing, Vector3.down,
                            invalidPointSpacing,
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

            foreach (PatrolPoint vector3 in pointsToRemove)
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
        return pointPositions.ElementAt(Random.Range(0, pointPositions.Count)).Position;
    }

    [ContextMenu("Sort Points")]
    public void SortPoints()
    {
        bool flag = false;

        pointPositions_Cover = new List<PatrolPoint>();
        pointPositions_Open = new List<PatrolPoint>();
        HashSet<PatrolPoint>.Enumerator e = pointPositions.GetEnumerator();
        int i;
        for (int j = 0; j < pointPositions.Count; j++)
        {
            i = 0;
            flag = false;
            PatrolPoint pointPosition = e.Current;
            if (pointPosition != null)
            {
                RaycastHit[] hits =
                    Physics.SphereCastAll(pointPosition.Position, coverSpacing, Vector3.down, coverSpacing,
                        coverLayerMask);
                while (i < hits.Length && !flag)
                {
                    if (coverTags.Contains(hits[i].collider.tag))
                    {
                        pointPositions_Cover.Add(pointPosition);
                        pointPosition.IsCover = true;
                        flag = true;
                    }

                    i++;
                }

                if (!flag)
                {
                    pointPositions_Open.Add(pointPosition);
                }
            }

            e.MoveNext();
        }
    }

    public List<PatrolPoint> GetPoints(Vector3 position, float range)
    {
        List<PatrolPoint> finalPoints = new List<PatrolPoint>();


        // finalPoints.Add(position);
        if (debug_ShowTempList)
        {
            debug_TempList = new List<Vector3>();
        }

        int count = 0;
        // Vector3 offset = new Vector3(range - pointSpacing, range - pointSpacing, range - pointSpacing);
        position = ConvertPoint(position);
        foreach (Vector3 p in ScanSurrounding((position), range * 2, range * 2,
            range * 2, true, position))
        {
            PatrolPoint pp = new PatrolPoint(p);
            if (pointPositions.Contains(pp))
            {
                finalPoints.Add(pp);
            }

            if (debug_ShowTempList)
            {
                debug_TempList.Add(p);
            }

            count++;
        }

        // print($"Found {finalPoints.Count} Matching Points");


        return finalPoints;
    }

    public Vector3 ConvertPoint(Vector3 point)
    {
        point /= pointSpacing;
        Vector3 temp = new Vector3(Mathf.Round(point.x), Mathf.Round(point.y), Mathf.Round(point.z));

        if (useWorldPositions)
        {
        }

        temp *= pointSpacing;
        if (!useWorldPositions)
        {
            Vector3 offset = startingPoint - new Vector3(Mathf.Round(startingPoint.x),
                Mathf.Round(startingPoint.y), Mathf.Round(startingPoint.z));
            temp += offset;
        }


        return temp;
    }


    float IsEvenFlip(float x)
    {
        if (x % 2 == 0)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}