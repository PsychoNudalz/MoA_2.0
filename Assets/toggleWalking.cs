using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class toggleWalking : MonoBehaviour
{
    Animator animator;
    bool IsWalking;
    Keyboard kb;

    void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
        animator = GetComponent<Animator>();
        IsWalking = animator.GetBool("IsWalking");
    }

    private void Update()
    {
        if (kb.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space");
            IsWalking = !IsWalking;
            animator.SetBool("IsWalking", IsWalking);
        }
        
    }
}
