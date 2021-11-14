using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent_Barrel : GunComponent
{
    [Header("Muzzle Location")]
    [SerializeField] Transform muzzleLocation;
    [Header("Hand Location")]
    [SerializeField] HandPositionPointer hpp_Left;
    [SerializeField] HandPositionPointer hpp_Right;

    public Transform MuzzleLocation { get => muzzleLocation; set => muzzleLocation = value; }
    public HandPositionPointer Hpp_Left { get => hpp_Left; set => hpp_Left = value; }
    public HandPositionPointer Hpp_Right { get => hpp_Right; set => hpp_Right = value; }
}