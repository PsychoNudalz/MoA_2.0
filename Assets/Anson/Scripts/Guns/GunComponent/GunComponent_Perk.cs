using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Perk))]
public class GunComponent_Perk : GunComponent
{
    [Header("Perk")]
    [SerializeField]
    private Perk perk;

    public Perk Perk => perk;

    protected override void AwakeBehaviour()
    {
        base.AwakeBehaviour();
        if (!perk)
        {
            perk = GetComponent<Perk>();
        }
    }
}
