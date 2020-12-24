using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentGunStatsScript : GunStatsScript
{
    [Header("Gun Stats Multiplier")]
    [Range(0.0000001f,10f)]
    [SerializeField] public float damagePerProjectileM = 1;
    [Range(0.0000001f, 10f)]
    [SerializeField] public float RPMM = 1;
    [Range(0.0000001f, 10f)]
    [SerializeField] public float reloadSpeedM = 1;
    [SerializeField] public Vector2 recoilM = new Vector2(1, 1);
    [Range(0.0000001f,10f)]
    [SerializeField] public float rangeM = 1;
    [SerializeField] public float magazineSizeM = 1;


}
