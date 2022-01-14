using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ElementDebuffScript : DebuffScript
{
    protected float effectDamage;
    protected float effectPotency;
    public LayerMask layerMask = new LayerMask();
    public List<string> tagList = new List<string>();

    public ElementDebuffScript()
    {

    }

    public virtual void init(float effectDamage, float effectPotency, List<string> tagList, LayerMask layerMask)
    {
        this.effectDamage = effectDamage;
        this.effectPotency = effectPotency;
        duration = effectPotency;
        Debug.Log("new element: " + effectDamage + "  " + effectPotency);
        this.tagList = tagList;
        this.layerMask = layerMask;

    }
}
