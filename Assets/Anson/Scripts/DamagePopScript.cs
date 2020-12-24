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
    public void displayDamage(float dmg)
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
    }

    bool checkText()
    {
        return text.gameObject.activeSelf;
    }
}
