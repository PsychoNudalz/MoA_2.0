using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageMultiplier
{

    public static float ElementDamageNerf { get => 0.85f; }

    public static float Get(GunTypes gt)
    {
        float temp = 1f;
        switch (gt)
        {
            case GunTypes.SMALL:
                temp = 2f;
                break;
            case GunTypes.RIFLE:
                temp = 1.5f;

                break;
            case GunTypes.SHOTGUN:
                temp = 1.2f;

                break;
            case GunTypes.EXPLOSIVE:
                temp = 1.5f;

                break;
            case GunTypes.SNIPER:
                temp = 2.5f;

                break;
            default:
                break;
        }
        return temp;
    }

    public static float Get(ElementTypes et)
    {
        float temp = 1f;

        switch (et)
        {
            case ElementTypes.PHYSICAL:
                break;
            case ElementTypes.FIRE:
                temp = 2f;
                break;
            case ElementTypes.ICE:
                break;
            case ElementTypes.SHOCK:
                break;
            default:
                break;
        }
        return temp;
    }

}