using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{
    [SerializeField]
    private PatrolZone[] patrolZones = Array.Empty<PatrolZone>();


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public PatrolZone[] PatrolZones => patrolZones;


    [ContextMenu("Initialise All")]
    public void InitialiseAllZones()
    {
        patrolZones = GetComponentsInChildren<PatrolZone>();
        foreach (PatrolZone patrolZone in patrolZones)
        {
            patrolZone.GenerateAll();
        }
    }

    [ContextMenu("Initialise All_Rebuild")]
    public void InitialiseAllZonesFromLists()
    {
        patrolZones = GetComponentsInChildren<PatrolZone>();
        foreach (PatrolZone patrolZone in patrolZones)
        {
            if (patrolZone.InitialiseDictionaryFromLists())
            {
                patrolZone.GenerateAll();
            }
        }
    }

    [ContextMenu("Initialise All_Soft")]
    public void InitialiseAllZones_Soft()
    {
        if (patrolZones.Length == 0)
        {
            patrolZones = GetComponentsInChildren<PatrolZone>();
        }

        foreach (PatrolZone patrolZone in patrolZones)
        {
            patrolZone.GenerateAll();
        }
    }

    [ContextMenu("ShowDebug_Enable")]
    public void ShowDebug_Enable()
    {
        foreach (PatrolZone patrolZone in patrolZones)
        {
            patrolZone.ShowDebug = true;
        }
    }

    [ContextMenu("ShowDebug_Disable")]
    public void ShowDebug_Disable()
    {
        foreach (PatrolZone patrolZone in patrolZones)
        {
            patrolZone.ShowDebug = false;
            patrolZone.DebugAutoRegenerate = false;
        }
    }

    public PatrolZone GetZone(int i)
    {
        int temp = i;
        i = Mathf.Min(Mathf.Max(0, i), patrolZones.Length - 1);
        if (temp != i)
        {
            Debug.LogWarning($"Patrol Manager Requested {temp} out of range.  Returned {i} instead.");
        }

        return patrolZones[i];
    }

    public int GetZoneIndex(PatrolZone patrolZone)
    {
        for (var i = 0; i < patrolZones.Length; i++)
        {
            PatrolZone zone = patrolZones[i];
            if (patrolZone.Equals(zone))
            {
                return i;
            }
        }

        return -1;
    }
}