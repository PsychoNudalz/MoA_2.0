using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public GameObject followTarget;

    private void Update()
    {
        transform.position = followTarget.transform.position;
        transform.rotation = followTarget.transform.rotation;
    }
}
