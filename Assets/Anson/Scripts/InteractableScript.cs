using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableScript : MonoBehaviour
{
    public bool interactableActive = true;
    [Header("Sound")]
    private SoundManager soundManager;
    public Sound Sound_active;
    public Sound Sound_deactive;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }
    public virtual void activate()
    {
        interactableActive = true;
        PlaySound_use();
    }

    public virtual void deactivate()
    {
        interactableActive = false;
        PlaySound_use();

    }

    public virtual void toggleActivate()
    {
        if (interactableActive)
        {
            deactivate();
        }
        else
        {
            activate();
        }
    }

    public void setActivationForList(List<InteractableScript> interactableScripts, bool b)
    {
        foreach (InteractableScript i in interactableScripts)
        {
            if (b)
            {
                i.activate();
            }
            else
            {
                i.deactivate();
            }
        }
    }
    public void toggleActivationForList(List<InteractableScript> interactableScripts)
    {
        foreach (InteractableScript i in interactableScripts)
        {
            i.toggleActivate();
        }
    }

    public virtual void PlaySound_use()
    {
        if (Sound_active == null)
        {
            return;
        }
        if (interactableActive || Sound_deactive == null)
        {
            soundManager.Play(Sound_active);
        }
        else if (!interactableActive)
        {
            soundManager.Play(Sound_deactive);
        }

    }
}
