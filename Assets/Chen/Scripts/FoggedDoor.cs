﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoggedDoor : MonoBehaviour
{
    void OnDrawGizmos() {
        Ray ray = new Ray (transform.position, transform.rotation * Vector3.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(ray);
    }
}
