using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anson:
/// script to play a group of particle system at once
/// </summary>
public class GroupParticleSystemScript : MonoBehaviour
{

    [SerializeField] List<ParticleSystem> particleSystems = new List<ParticleSystem>();


    private void Awake()
    {
        autoSetList();
    }

    void autoSetList()
    {
        particleSystems = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
    }

    /// <summary>
    /// Play all particle system
    /// </summary>
    public void Play()
    {

        foreach (ParticleSystem p in particleSystems)
        {
            p.Stop();
            p.Play();
        }
    }

}
