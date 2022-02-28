using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Perk))]
public class GunComponent_Perk : GunComponent
{
    [Header("Perk")]
    [SerializeField]
    private Perk perk;

    [SerializeField]
    private bool isStackable = true;

    public Perk Perk => perk;

    public bool IsStackable => isStackable;

    protected override void AwakeBehaviour()
    {
        base.AwakeBehaviour();
        AssignPerk();
    }

    [ContextMenu("Assign Perk")]
    public void AssignPerk()
    {
        if (!perk)
        {
            perk = GetComponent<Perk>();
        }
    }
}