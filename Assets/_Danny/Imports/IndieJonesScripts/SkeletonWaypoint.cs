using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonWaypoint : MonoBehaviour
{

    [Header("Time to perform idle animation")]
    [SerializeField]
    private float idleTime = 1f;
    [Header("Is skeleton running to next waypoint?")]
    [SerializeField]
    private bool isRunning;

    public float GetIdleTime()
    {
        return idleTime;
    }

    public bool GetIsRunning()
    {
        return isRunning;
    }
}
