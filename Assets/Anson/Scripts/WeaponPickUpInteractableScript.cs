using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUpInteractableScript : InteractableScript
{
    [SerializeField] MainGunStatsScript connectedGun;

    public MainGunStatsScript ConnectedGun { get => connectedGun; set => connectedGun = value; }
}
