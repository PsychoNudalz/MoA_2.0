using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZLock :MonoBehaviour
{
    [SerializeField] bool zLock;

    public static CameraZLock Current;
    private void Awake()
    {
        Current = this;
    }


    private void LateUpdate()
    {
        Vector3 temp = transform.localEulerAngles;
        if (zLock)
        {
            temp.z = 0;
        }
        transform.localEulerAngles = temp;
    }
}
