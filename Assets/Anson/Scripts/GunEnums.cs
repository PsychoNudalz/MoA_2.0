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
    MUZZLE
}

public enum GunTypes
{
    LOW,        //Pistol
    MID,        //Rifle
    HIGH,       //Shotgun, Sniper
    EXPLOSIVE   //Rocket, Grenade Launcher
}


public enum ElementTypes
{
    PHYSICAL,
    FIRE,
    ICE,
    EARTH
}
