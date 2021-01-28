using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraFollowObject : MonoBehaviour
{
    public Transform followObject;
    public Vector3 offset;
    

    // Update is called once per frame
    void Update()
    {
        this.transform.position = followObject.position - offset;
    }
}
