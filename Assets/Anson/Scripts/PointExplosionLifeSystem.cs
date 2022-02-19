using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointExplosionLifeSystem : LifeSystemScript
{
    [SerializeField]
    private PointExplosionSphere pointExplosionSphere;
    
    protected override void OnEnableBehaviour()
    {
        base.OnEnableBehaviour();
        ResetSystem();
    }

    public override int takeDamage(float dmg, int level, ElementTypes element, bool displayTakeDamageEffect = true)
    {
        pointExplosionSphere.Explode(dmg, level);

        return 0;
    }
}
