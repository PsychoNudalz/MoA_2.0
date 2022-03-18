using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;
    [SerializeField]
    protected TargetSoundScript soundScript;
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

   
}
