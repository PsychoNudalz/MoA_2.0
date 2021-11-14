using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Anson:
/// class for displaying damage dealt on the target
/// </summary>
public class DamagePopUpScript : MonoBehaviour
{
    public GameObject text;
    public Animator animator;
    public string displayText;
    private DamagePopUpUIScript pairedUI;

    [Header("Text colours")]
    [SerializeField] Color normalColour = Color.white;
    [SerializeField] Color critColour = Color.red;
    [SerializeField] Color fireColour = new Color(255, 235, 0);
    [SerializeField] Color iceColour = Color.cyan;
    [SerializeField] Color shockColour = Color.yellow;





    /// <summary>
    /// display the damage dealt to the target
    /// the total damage value stacks up until it disappears
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void displayDamage(float dmg, ElementTypes e = ElementTypes.PHYSICAL)
    {
        switch (e)
        {
            case (ElementTypes.PHYSICAL):
                displayDamage(dmg, normalColour);
                break;
            case (ElementTypes.FIRE):
                displayDamage(dmg, fireColour);
                break;
            case (ElementTypes.ICE):
                displayDamage(dmg, iceColour);
                break;
            case (ElementTypes.SHOCK):
                displayDamage(dmg, shockColour);
                break;
        }
    }


    /// <summary>
    /// display the damage dealt to the target
    /// the total damage value stacks up until it disappears
    /// text colour change depending on colour
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void displayDamage(float dmg, Color colour)
    {

        animator.SetTrigger("Play");
        displayText = Mathf.RoundToInt(dmg).ToString();
        pairedUI = DamagePopUpUIManager.current.displayDamage(displayText, colour, this);

    }


    /// <summary>
    /// display the critical damage dealt to the target
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void displayCriticalDamage(float dmg)
    {
        displayDamage(dmg, critColour);

    }

    public bool checkText()
    {
        return text.activeSelf;
    }


}
