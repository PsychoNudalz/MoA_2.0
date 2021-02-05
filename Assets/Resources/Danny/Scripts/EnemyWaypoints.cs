using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoints : MonoBehaviour
{
    public EnemyWaypoint[] waypoints;

    private void Start()
    {
        waypoints = GetComponentsInChildren<EnemyWaypoint>();
    }

    public EnemyWaypoint[] GetWaypointsToFollow()
    {
        return waypoints;
    }
}
