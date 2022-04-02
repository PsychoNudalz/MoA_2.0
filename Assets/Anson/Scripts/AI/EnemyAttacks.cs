using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected TargetSoundScript soundScript;

    [SerializeField]
    protected AILogic aiLogic;
    
    //Getters and Setters
    public TargetSoundScript SoundScript
    {
        get => soundScript;
        set => soundScript = value;
    }

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

    public AILogic AILogic
    {
        get => aiLogic;
        set => aiLogic = value;
    }
}
