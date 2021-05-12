using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsHandler : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private bool speedUp = false;

    public void ExitOnClick() {
        SceneManager.LoadScene("MainEntry");
    }

    public void ToggleOnClick() {
        if (speedUp) {
            animator.speed = 1;
        } else {
            animator.speed = 2;
        }
        speedUp = !speedUp;
    }
    
}
