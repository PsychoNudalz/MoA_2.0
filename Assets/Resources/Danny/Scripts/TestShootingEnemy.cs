using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestShootingEnemy : MonoBehaviour
{
    Animator animator;
    Keyboard kb;

    void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (kb.wKey.wasPressedThisFrame)
        {
            animator.SetBool("IsWalking", !animator.GetBool("IsWalking"));
        }

        if (kb.dKey.wasPressedThisFrame)
        {
            animator.SetBool("IsDead", true);
        }

        if (kb.cKey.wasPressedThisFrame)
        {
            animator.SetBool("IsCrouching", !animator.GetBool("IsCrouching"));
        }

        if (kb.sKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Shoot");
        }

        if (kb.hKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Hit");
        }

    }
}
