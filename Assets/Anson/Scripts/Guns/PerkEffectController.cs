using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkEffectController : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField]
    private Sound activateSound;

    [SerializeField]
    private Sound deactivateSound;

    public void PlayActivate()
    {
        activateSound?.PlayF();
    }
    
    public void PlayDeactivate()
    {
        deactivateSound?.PlayF();
    }
}