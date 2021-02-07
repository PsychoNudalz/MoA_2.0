using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoint : MonoBehaviour
{
    /*
     * Represents if waypoint is already assigned to an enemy
     */
    private bool isValid = true;

    public bool GetIsValid()
    {
        return isValid;
    }

    public void SetIsValid(bool isValid)
    {
        this.isValid = isValid;
    }
}
