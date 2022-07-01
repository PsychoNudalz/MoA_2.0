using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using Random = UnityEngine.Random;


/// <summary>
/// Anson:
/// class for displaying damage dealt on the target
/// </summary>
public class DamagePopUpScript : MonoBehaviour
{
    [SerializeField]
    float spawnRange = 1f;

    public string displayText;
    private DamagePopUpUIScript pairedUI;


    private DamagePopUpUIManager damagePopUpUIManager;


    private void Start()
    {
        damagePopUpUIManager = DamagePopUpUIManager.current;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(200, 200, 200, 100);
        Gizmos.DrawSphere(transform.position,spawnRange);
    }

    /// <summary>
    /// display the damage dealt to the target
    /// the total damage value stacks up until it disappears
    /// </summary>
    /// <param name="dmg"></param>
    public virtual void displayDamage(float dmg, ElementTypes e = ElementTypes.PHYSICAL)
    {
        damagePopUpUIManager.displayDamage(dmg.ToString("0"),e,transform.position+GetRandomPosition());
    }

    protected Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-spawnRange, spawnRange),
            Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
    }
}