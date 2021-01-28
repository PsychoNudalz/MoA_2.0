using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCameraFollow : MonoBehaviour
{
    public Transform followObject;
    private Vector3 offset;

    private void Start()
    {
        offset = followObject.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = followObject.position - offset;
    }
}
