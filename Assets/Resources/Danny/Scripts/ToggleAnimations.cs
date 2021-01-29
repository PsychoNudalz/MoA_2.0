using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ToggleAnimations : MonoBehaviour
{
    Animator animator;
    bool IsWalking;
    bool IsDead;
    Keyboard kb;

    void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        animator = GetComponent<Animator>();
        IsWalking = animator.GetBool("IsWalking");
    }

    private void Update()
    {
        
        if (kb.hKey.wasPressedThisFrame)
        {
            GetComponent<StoneEnemyLifeSystem>().takeDamage(1, 1, ElementTypes.PHYSICAL);
        }

    }
}
