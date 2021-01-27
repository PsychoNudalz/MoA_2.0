using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandlerScript : MonoBehaviour
{
    [SerializeField] TargetLifeSystem targetLifeSystem;
    [SerializeField] TargetMaterialHandlerScript targetMaterialHandler;

    public TargetLifeSystem TargetLifeSystem { get => targetLifeSystem; }
    public TargetMaterialHandlerScript TargetMaterialHandler { get => targetMaterialHandler; }

    private void Awake()
    {
        if (targetLifeSystem == null)
        {
            targetLifeSystem = GetComponent<TargetLifeSystem>();
        }
        if (targetMaterialHandler == null)
        {
            targetMaterialHandler = GetComponent<TargetMaterialHandlerScript>();
        }
    }

}


