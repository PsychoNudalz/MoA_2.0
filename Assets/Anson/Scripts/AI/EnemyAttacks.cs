using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    [SerializeField]
    protected Animator animator;

    public Animator Animator
    {
        get => animator;
        set => animator = value;
    }

   
}
