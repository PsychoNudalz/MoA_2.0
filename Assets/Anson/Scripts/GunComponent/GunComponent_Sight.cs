using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent_Sight : GunComponent
{
    [SerializeField] Transform sightLocation;
    [Header("Sight Stats")]
    [SerializeField] float zoomMultiplier = 1f;
    [SerializeField] MeshRenderer sightRenderer;
    [SerializeField] Material sightMaterial;

    public Transform SightLocation { get => sightLocation; set => sightLocation = value; }
    public float ZoomMultiplier { get => zoomMultiplier; set => zoomMultiplier = value; }

    public void SetSightMaterial(bool l)
    {
        if (!sightMaterial && sightRenderer)
        {
            sightMaterial = sightRenderer.material;
        }

        if (sightMaterial)
        {
            if (l)
            {
                sightMaterial.SetInt("_RearSight_ScreenLock", 1);

            }
            else
            {
            sightMaterial.SetInt("_RearSight_ScreenLock", 0);
            }
        }
    }

}