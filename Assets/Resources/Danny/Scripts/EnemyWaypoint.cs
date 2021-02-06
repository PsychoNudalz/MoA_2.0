using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaypoint : MonoBehaviour
{
    private bool isValid = true;

    public bool IsValid()
    {
        return isValid;
    }

    public void SetIsValid(bool isValid)
    {
        this.isValid = isValid;
    }
}
