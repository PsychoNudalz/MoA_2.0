using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHandlerScript : MonoBehaviour
{
    [SerializeField] TargetLifeSystem targetLifeSystem;
    [SerializeField] TargetEffectController targetMaterialHandler;
    [SerializeField] TargetSoundScript targetSoundScript;

    public TargetLifeSystem TargetLifeSystem { get => targetLifeSystem; }
    public TargetEffectController TargetMaterialHandler { get => targetMaterialHandler; }
    public TargetSoundScript TargetSoundScript { get => targetSoundScript; set => targetSoundScript = value; }

    private void Awake()
    {
        if (targetLifeSystem == null)
        {
            targetLifeSystem = GetComponent<TargetLifeSystem>();
        }
        if (targetMaterialHandler == null)
        {
            targetMaterialHandler = GetComponentInChildren<TargetEffectController>();
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

    public virtual Vector3 GetEffectCenter()
    {
        return targetLifeSystem.GetEffectCenter();
    }
}


