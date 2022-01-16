using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField]
    private bool isObstructed;

    [SerializeField]
    private List<string> tagList;

    [SerializeField]
    private List<Collider> obstructedColliders = new List<Collider>();

    public bool IsObstructed => isObstructed;

    private void OnTriggerEnter(Collider other)
    {
        if (tagList.Count==0|| tagList.Contains(other.tag))
        {
            if (!obstructedColliders.Contains(other))
            {
                obstructedColliders.Add(other);
            }
            isObstructed = obstructedColliders.Count>0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tagList.Count==0||tagList.Contains(other.tag))
        {
            if (obstructedColliders.Contains(other))
            {
                obstructedColliders.Remove(other);
            }
            isObstructed = obstructedColliders.Count>0;
        }
    }
}
