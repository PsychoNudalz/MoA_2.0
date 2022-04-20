using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UniversalValues
{
    [Header("ElementalColours")]
    private static Color shockColour = new Color(255,124,0);
    private static Color iceColour = new Color(0,255,255);
    private static Color fireColour = new Color(255,235,14);
    private static Color normalColour = new Color(255,255,255);
    
    //Weapon Handling Globals
    public static float HipFireRecoilMultiplier
    {
        get => 0.65f;
    }
    public static float ShotgunPelletRecoilMultiplier
    {
        get => 5f;
    }

    //Damage Multipliers
    public static float ElementDamageNerf
    {
        get => 0.85f;
    }

    public static float GetDamageMultiplier(GunTypes gt)
    {
        float temp = 1f;
        switch (gt)
        {
            case GunTypes.SMALL:
                temp = 2f;
                break;
            case GunTypes.RIFLE:
                temp = 1.7f;

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

    public static float GetDamageMultiplier(ElementTypes et)
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

    public static Color GetColour(ElementTypes elementTypes)
    {
        switch (elementTypes)
        {
            case ElementTypes.PHYSICAL:
                return normalColour;
                break;
            case ElementTypes.FIRE:
                return fireColour;
                break;
            case ElementTypes.ICE:
                return iceColour;
                break;
            case ElementTypes.SHOCK:
                return shockColour;
                break;
            default:
                return normalColour;
        }

        return normalColour;
    }
}