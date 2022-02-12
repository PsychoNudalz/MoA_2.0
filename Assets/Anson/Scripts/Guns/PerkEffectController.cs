using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PerkEffectController : MonoBehaviour
{
    [Header("VFX")]
    [SerializeField]
    private UnityEvent onActiveEffect;
    [SerializeField]
    private UnityEvent onDeactiveEffect;
    [Header("Sound")]
    [SerializeField]
    private Sound activateSound;

    [SerializeField]
    private Sound deactivateSound;

    private void Start()
    {
        
    }

    [ContextMenu("Set Sprite")]
    public void SetSprite()
    {
        Perk temp = GetComponent<Perk>();
        if (temp==null)
        {
            return;
        }
        SetSprite(temp.PerkSprite);
    }
    public void SetSprite(Sprite sprite)
    {
        
        var croppedTexture = new Texture2D( (int)sprite.rect.width, (int)sprite.rect.height );
        var pixels = sprite.texture.GetPixels(  (int)sprite.textureRect.x, 
            (int)sprite.textureRect.y, 
            (int)sprite.textureRect.width, 
            (int)sprite.textureRect.height );
        croppedTexture.SetPixels( pixels );
        croppedTexture.Apply();
        foreach (VisualEffect visualEffect in GetComponentsInChildren<VisualEffect>())
        {
            if (visualEffect.HasTexture("PerkSprite"))
            {
                visualEffect.SetTexture("PerkSprite", croppedTexture);
            }
        }
    }

    public void PlayActivate()
    {
        onActiveEffect.Invoke();
        activateSound?.PlayF();
    }
    
    public void PlayDeactivate()
    {
        onDeactiveEffect.Invoke();
        deactivateSound?.PlayF();
    }
}