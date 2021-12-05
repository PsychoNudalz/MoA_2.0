using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float footStepFrequency_Walk = 0.5f;

    [SerializeField]
    private float footStepFrequency_Crouch = 1f;

    private float lastFootStepTime = 0;
    private bool isWalking = false;
    private bool isCrouch = false;

    [Header("Sounds")]
    [SerializeField]
    Sound dashSound;

    [SerializeField]
    Sound jumpSound;

    [SerializeField]
    Sound takeDamageSound;

    [SerializeField]
    Sound healSound;

    [SerializeField]
    private Sound walkSound;

    [SerializeField]
    private Sound landSound;

    [SerializeField]
    private Sound slideSound;
    // Start is called before the first frame update

    private void Awake()
    {
    }

    private void FixedUpdate()
    {
        if (isWalking)
        {
            if (isCrouch)
            {
                if (Time.time - lastFootStepTime > footStepFrequency_Crouch)
                {
                    Play_Walk();
                    lastFootStepTime = Time.time;
                }
            }
            else
            {
                if (Time.time - lastFootStepTime > footStepFrequency_Walk)
                {
                    Play_Walk();
                    lastFootStepTime = Time.time;
                }
            }
        }
    }

    public void Play_Dash()
    {
        dashSound.PlayF();
    }

    public void Play_Jump()
    {
        jumpSound.PlayF();
    }

    public void Play_TakeDamage()
    {
        takeDamageSound.PlayF();
    }

    public void Play_Heal()
    {
        healSound.PlayF();
    }

    public void Play_Walk()
    {
        walkSound.PlayF();
    }

    public void Set_Walk(bool b)
    {
        isWalking = b;
    }
    public void Set_Crouch(bool b)
    {
        isCrouch = b;
    }

    public void Play_Slide()
    {
        slideSound.PlayF();
    }

    public void Stop_Slide()
    {
        slideSound.Stop();
    }

    public void Play_Land()
    {
        landSound.PlayF();
    }
}