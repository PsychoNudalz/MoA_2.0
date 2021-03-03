using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunComponents
{
    BODY,
    GRIP,
    MAGAZINE,
    BARREL,
    STOCK,
    SIGHT,
    ATTACHMENT,
    MUZZLE,
    STATBOOST
}

public enum GunTypes
{
    SMALL,        //Pistol
    RIFLE,        //Rifle
    SHOTGUN,       //Shotgun, Sniper
    EXPLOSIVE,   //Rocket, Grenade Launcher
    SNIPER

}


public enum ElementTypes
{
    PHYSICAL,
    FIRE,
    ICE,
    SHOCK
}

public enum FireTypes
{
    HitScan,
    Projectile
}

public enum Rarity
{
    COMMON,
    UNCOMMON,
    RARE,
    LEGENDARY,
    EXOTIC
}
