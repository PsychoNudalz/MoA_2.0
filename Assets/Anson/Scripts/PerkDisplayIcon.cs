using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PerkDisplayIcon : MonoBehaviour
{
    [SerializeField]
    private Sprite perkSprite;

    [SerializeField]
    private int stackValue = 0;

    [SerializeField]
    private Perk connectedPerk;

    [Header("UI")]
    [SerializeField]
    private Image perkImage;

    [SerializeField]
    private TextMeshProUGUI perkStackText;

    [SerializeField]
    private Animator spriteAnimator;

    [SerializeField]
    private string startAnimationName = "Start_1";
    
    // [Header("Update ")]

    public Sprite PerkSprite
    {
        get => perkSprite;
        set => perkSprite = value;
    }

    private void Awake()
    {
        perkImage.material = new Material(perkImage.material);
        if (!spriteAnimator)
        {
            spriteAnimator = GetComponentInChildren<Animator>();
        }
        
    }

    public void SetPerk(Perk p)
    {
        connectedPerk = p;
        SetSprite(p.PerkSprite);
        SetStack(p.StackCurrent);
    }

    public void SetSprite(Sprite s)
    {

        perkSprite = s;
        perkImage.sprite = perkSprite;
    }

    public void SetSprite()
    {
        spriteAnimator.enabled = false;
        perkImage.sprite = perkSprite;

    }

    public void SetStack(int i)
    {
        stackValue = i;
        if (i <= 1)
        {
            perkStackText.text = "";

        }
        else
        {
            perkStackText.text = $"x{stackValue}";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (connectedPerk)
        {
            perkImage.material.SetFloat("_TimeValue",connectedPerk.GetDurationFraction());
        }
    }

    private void OnEnable()
    {
        spriteAnimator.enabled = true;

        spriteAnimator.Play(startAnimationName);
    }
}
