using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointExplosionPoint : MonoBehaviour
{
    [SerializeField]
    private float distanceDeadZone = 0.01f;

    [SerializeField]
    private LineRenderer[] lineRenderers = new LineRenderer[] { };

    [SerializeField]
    private int indexPoint = 0;

    [SerializeField]
    private List<Vector3> positionPoints;

    [ContextMenu("Add Line renderer")]
    public void SetLineRenderer()
    {
        lineRenderers = GetComponentsInChildren<LineRenderer>();
    }


    public void SetPositionPoint(List<Vector3> points)
    {
        positionPoints = points;
        UpdatePositionPointAll();
    }

    public void SetPositionPoint(Vector3 point)
    {
        positionPoints.Add(point);
        UpdatePositionPoint(point);
    }

    void UpdatePositionPointAll()
    {
        foreach (Vector3 point in positionPoints)
        {
            if (Vector3.Distance(point, transform.position) > distanceDeadZone)
            {
                UpdatePositionPoint(point);
            }
        }
    }

    void UpdatePositionPoint(Vector3 point)
    {
        lineRenderers[indexPoint].SetPosition(0, transform.position);
        lineRenderers[indexPoint].SetPosition(1, point);
        indexPoint = (indexPoint + 1) % lineRenderers.Length;
    }

    public void ResetOldestPoint()
    {
        lineRenderers[(indexPoint +lineRenderers.Length-1) % lineRenderers.Length].SetPosition(0, transform.position);
        lineRenderers[(indexPoint+ lineRenderers.Length-1) % lineRenderers.Length].SetPosition(1, transform.position);
    }

    public void ResetAllPoints()
    {
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position);

        }
    }
}