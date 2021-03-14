using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class vfx_GetCameraPositionScript : MonoBehaviour
{
    [SerializeField] VisualEffect vfx;
    [SerializeField] Camera camera;

    private void Start()
    {
        vfx = GetComponent<VisualEffect>();
        camera = FindObjectOfType<Camera>();
        if (!vfx)
        {
            Debug.LogError("Failed to find vfx");
        }
    }

    private void FixedUpdate()
    {

        vfx.SetVector3("Main Camera Position", transform.InverseTransformPoint(camera.transform.position));
    }
}
