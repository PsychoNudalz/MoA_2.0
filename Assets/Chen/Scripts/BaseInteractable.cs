using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseInteractable : UIPopUpInteractableScript
{
    // Keyboard keyboard;

    // private void Awake()
    // {
    //     keyboard = InputSystem.GetDevice<Keyboard>();
    // }

    // private void Update()
    // {
    //     if (keyboard.eKey.wasReleasedThisFrame && toolTip.activeSelf && linkedUI != null && !linkedUI.activeSelf)
    //     {
    //         Cursor.visible = true;
    //         Cursor.lockState = CursorLockMode.Confined;
    //         if (linkedUI != null)
    //         {
    //             linkedUI.SetActive(true);
    //         }
    //     }
    // }

    public override void activate()
    {
        base.activate();
    }

    public override void deactivate()
    {
        base.deactivate();
    }

}
