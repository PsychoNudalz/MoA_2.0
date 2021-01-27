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
        if (kb.wKey.wasPressedThisFrame)
        {
            IsWalking = !IsWalking;
            animator.SetBool("IsWalking", IsWalking);
        }

        if (kb.dKey.wasPressedThisFrame)
        {
            animator.SetBool("IsDead", true);
        }

        if (kb.hKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Hit");
        }

        if (kb.aKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Attack");
        }
    }
}
