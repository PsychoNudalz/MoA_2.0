using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandlerScript : MonoBehaviour
{
    [SerializeField] TargetLifeSystem targetLifeSystem;
    [SerializeField] TargetMaterialHandlerScript targetMaterialHandler;
    [SerializeField] TargetSoundScript targetSoundScript;

    public TargetLifeSystem TargetLifeSystem { get => targetLifeSystem; }
    public TargetMaterialHandlerScript TargetMaterialHandler { get => targetMaterialHandler; }
    public TargetSoundScript TargetSoundScript { get => targetSoundScript; set => targetSoundScript = value; }

    private void Awake()
    {
        if (targetLifeSystem == null)
        {
            targetLifeSystem = GetComponent<TargetLifeSystem>();
        }
        if (targetMaterialHandler == null)
        {
            targetMaterialHandler = GetComponentInChildren<TargetMaterialHandlerScript>();
        }
        if (targetSoundScript == null)
        {
            targetSoundScript = GetComponentInChildren<TargetSoundScript>();
        }
    }

    private void OnEnable()
    {
        PlaySpawnEffects();
    }

    private void PlaySpawnEffects()
    {
        targetSoundScript.Play_Spawn();
        targetMaterialHandler.SpawnEffect();
    }
}


