using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent_Sight : GunComponent
{
    [SerializeField] Transform sightLocation;

    public Transform SightLocation { get => sightLocation; set => sightLocation = value; }


}