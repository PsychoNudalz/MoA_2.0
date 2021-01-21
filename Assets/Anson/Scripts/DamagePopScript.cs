using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


/// <summary>
/// Anson:
/// class for displaying damage dealt on the target
/// </summary>
public class DamagePopScript : MonoBehaviour
{
    public TextMeshPro text;
    public Animator animator;
    public float value;
    public string displayText;
    [SerializeField] Camera camera;

    [Header("Text colours")]
    [SerializeField] Color normalColour = Color.white;
    [SerializeField] Color critColour = Color.yellow;
    [SerializeField] Color fireColour = Color.red;
    [SerializeField] Color iceColour = Color.cyan;
    [SerializeField] Color shockColour = Color.yellow;


    private void FixedUpdate()
    {
        rotateTextToCamera();
    }

    void rotateTextToCamera()
    {
        if (camera == null)
        {
            camera = FindObjectOfType<Camera>();
        }
        Vector3 dir = camera.transform.position - transform.position;
        transform.forward = -dir;
    }


    /// <summary>
    /// display the damage dealt to the target
    /// the total damage value stacks up until it disappears
    /// </summary>
    /// <param name="dmg"></param>
    public void displayDamage(float dmg,ElementTypes e = ElementTypes.PHYSICAL)
    {
        switch (e)
        {
            case (ElementTypes.PHYSICAL):
                displayDamage(dmg,normalColour);
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
    public void displayDamage(float dmg,Color colour)
    {
        if (!checkText())
        {
            value = 0;
        }
        text.gameObject.SetActive(true);
        animator.SetTrigger("Play");
        value += dmg;
        displayText = Mathf.RoundToInt(value).ToString();
        text.text = displayText;
        text.color = colour;
    }


    /// <summary>
    /// display the critical damage dealt to the target
    /// </summary>
    /// <param name="dmg"></param>
    public void displayCriticalDamage(float dmg)
    {
        if (!checkText())
        {
            value = 0;
        }
        text.gameObject.SetActive(true);
        animator.SetTrigger("Play");
        value += dmg;
        displayText = Mathf.RoundToInt(value).ToString();
        text.text = displayText;
        text.color = critColour;
    }

    bool checkText()
    {
        return text.gameObject.activeSelf;
    }
}
