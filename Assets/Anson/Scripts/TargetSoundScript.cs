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
    [SerializeField] Sound spawnSound;

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

    public void Play_Stagger(bool b = true)
    {
        if (!staggerSound)
        {
            return;
        }

        if (b)
        {
            staggerSound.PlayF();
        }

        else
        {
            staggerSound.Stop();
        }
    }

    public void Play_Death()
    {
        if (!deathSound)
        {
            return;
        }
        deathSound.Play();
    }
    public void Play_Spawn()
    {
        if (!spawnSound)
        {
            return;
        }
        spawnSound.Play();
    }
}
