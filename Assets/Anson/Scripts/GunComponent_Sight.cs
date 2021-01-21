using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent_Sight : GunComponent
{
    [SerializeField] Transform sightLocation;
    [Header("Sight Stats")]
    [SerializeField] float zoomMultiplier = 1f;
    [SerializeField] Material sightMaterial;
    [SerializeField] Sprite aimSprite;

    public Transform SightLocation { get => sightLocation; set => sightLocation = value; }
    public float ZoomMultiplier { get => zoomMultiplier; set => zoomMultiplier = value; }
}