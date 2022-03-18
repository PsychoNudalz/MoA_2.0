using System;
using System.Collections;
using System.Collections.Generic;
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

[ContextMenu("Initialise All")]
    public void InitialiseAllZones()
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
}
