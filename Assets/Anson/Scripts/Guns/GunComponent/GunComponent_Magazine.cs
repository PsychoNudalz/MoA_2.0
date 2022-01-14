using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent_Magazine : GunComponent
{
    [SerializeField] GameObject projectile;

    public GameObject Projectile { get => projectile; set => projectile = value; }
}