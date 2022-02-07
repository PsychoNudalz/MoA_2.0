using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotDataManager : MonoBehaviour
{
    public static ShotDataManager current;

    [SerializeField]
    private ShotData recentShot;

    public ShotData RecentShot
    {
        get => recentShot;
        set => recentShot = value;
    }

    [SerializeField]
    private List<ShotData> currentMag;

    [SerializeField]
    private List<ShotData> previousMag;

    public List<ShotData> CurrentMag => currentMag;

    public List<ShotData> PreviousMag => previousMag;

    private void Awake()
    {
        if (current)
        {
            DestroyImmediate(current);
            current = null;
        }

        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public static void Add(ShotData sd)
    {
        if (current)
        {
            current?.currentMag.Add(sd);
            current.RecentShot = sd;
        }
        else
        {
            Debug.LogWarning("Missing ShowDataManager");
        }
    }

    public static void Reset()
    {
        if (current)
        {
            current.previousMag = new List<ShotData>(current.currentMag.ToArray());
            
            current.currentMag = new List<ShotData>();
        }
    }
}