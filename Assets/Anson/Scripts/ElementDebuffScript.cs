using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementDebuffScript : DebuffScript
{
    protected float effectDamage;
    protected float effectPotency;

    public ElementDebuffScript(float effectDamage, float effectPotency)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = effectPotency;
        Debug.Log("new element: " + effectDamage + "  "+ effectPotency);
    }
}
