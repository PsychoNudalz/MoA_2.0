using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventInteractable : InteractableScript
{
    [SerializeField]
    private UnityEvent activateUE;
    
    [SerializeField]
    private UnityEvent deactivateUE;
    public override void activate()
    {
        base.activate();
        activateUE.Invoke();
    }

    public override void deactivate()
    {
        base.deactivate();
        deactivateUE.Invoke();

    }
}