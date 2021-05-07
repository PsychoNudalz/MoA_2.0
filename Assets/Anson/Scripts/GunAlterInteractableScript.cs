using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAlterInteractableScript : UIPopUpInteractableScript
{
    public override void deactivate()
    {
        base.deactivate();
        GetComponent<GunAlterUIHandler>().CloseMenu();
    }
}
