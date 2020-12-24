using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLifeSystem : LifeSystemScript
{
    [Header("Shader Effects")]
    [SerializeField] DecayShaderScript decayShaderScript;

    public override int takeDamage(float dmg, int level, ElementTypes element)
    {
        decayShaderScript.StartDecay();
        return base.takeDamage(dmg, level, element);
    }
}
