using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSoundScript : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] Sound attackSound;
    [SerializeField] Sound takeDamageSound;
    [SerializeField] Sound staggerSound;
    [SerializeField] Sound deathSound;

    public void Play_Attack()
    {
        if (!attackSound)
        {
            return;
        }
        attackSound.Play();
    }

    public void Play_TakeDamage()
    {
        if (!takeDamageSound)
        {
            return;
        }
        takeDamageSound.PlayF();
    }

    public void Play_Stagger()
    {
        if (!staggerSound)
        {
            return;
        }
        staggerSound.PlayF();
    }

    public void Play_Death()
    {
        if (!deathSound)
        {
            return;
        }
        deathSound.Play();
    }
}
