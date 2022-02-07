using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShotData
{
    [SerializeField]
    private Vector3 playerPos= new Vector3();
    [SerializeField]
    private Vector3 hitPos= new Vector3();
    [SerializeField]
    private LifeSystemScript targetLS = null;
    [SerializeField]
    private Vector3 targetPos = new Vector3();
    [SerializeField]
    private bool isHit = false;
    [SerializeField]
    private bool isTargetHit = false;
    [SerializeField]
    private bool isCritical = false;
    [SerializeField]
    private bool isKill = false;
    [SerializeField]
    private float shotDamage = 0f;
    [SerializeField]
    private bool isElementTrigger = false;
    

    public Vector3 PlayerPos
    {
        get => playerPos;
        set => playerPos = value;
    }

    public Vector3 HitPos
    {
        get => hitPos;
        set => hitPos = value;
    }

    public LifeSystemScript TargetLs
    {
        get => targetLS;
        set => targetLS = value;
    }

    public Vector3 TargetPos
    {
        get => targetPos;
        set => targetPos = value;
    }

    public bool IsHit
    {
        get => isHit;
        set => isHit = value;
    }

    public bool IsTargetHit
    {
        get => isTargetHit;
        set => isTargetHit = value;
    }

    public bool IsCritical
    {
        get => isCritical;
        set => isCritical = value;
    }

    public bool IsKill
    {
        get => isKill;
        set => isKill = value;
    }

    public float ShotDamage
    {
        get => shotDamage;
        set => shotDamage = value;
    }

    public bool IsElementTrigger
    {
        get => isElementTrigger;
        set => isElementTrigger = value;
    }

    public ShotData()
    {
    }   public ShotData(Vector3 pos)
    {
        playerPos = pos;
    }

    public ShotData(LifeSystemScript targetLs, bool isHit, bool isTargetHit, bool isCritical, bool isKill, float shotDamage, bool isElementTrigger)
    {
        targetLS = targetLs;
        this.isHit = isHit;
        this.isTargetHit = isTargetHit;
        this.isCritical = isCritical;
        this.isKill = isKill;
        this.shotDamage = shotDamage;
        this.isElementTrigger = isElementTrigger;
    }

    public void SetLifeSystem(LifeSystemScript ls)
    {
        targetLS = ls;
        targetPos = ls.transform.position;
    }
}


