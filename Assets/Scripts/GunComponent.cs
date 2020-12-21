﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunComponent : MonoBehaviour
{
    [SerializeField] private GunComponents componentType = GunComponents.ATTACHMENT;
    [SerializeField] private List<GunTypes> gunTypes;
    //[SerializeField] protected List<GunComponent> connectedComponents;
    [SerializeField] protected List<GunConnectionPoint> gunConnectionPoints;
    [SerializeField] protected ComponentGunStatsScript componentGunStatsScript;

    protected GunComponents ComponentType { get => componentType;}
    protected List<GunTypes> GTypes { get => gunTypes;}


    private void Awake()
    {
        componentGunStatsScript = GetComponent<ComponentGunStatsScript>();
    }

    public GunComponents GetGunComponentType()
    {
        return ComponentType;
    }

    public List<GunTypes> GetGunTypes()
    {
        return new List<GunTypes>(GTypes);
    }

    public List<GunConnectionPoint> GetGunConnectionPoints()
    {
        return gunConnectionPoints;

    }

}
