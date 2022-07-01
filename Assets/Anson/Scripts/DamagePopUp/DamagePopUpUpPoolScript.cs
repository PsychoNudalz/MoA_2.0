using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class DamagePopUpUpPoolScript : DamagePopUpScript
{
    // [SerializeField] List<DamagePopUpScript> DPPool;
    // [SerializeField] DamagePopUpScript baseDP;
    // [SerializeField] int pointer = 0;
    // [SerializeField] int initialSize = 10;
    // float timeNow = 0;
    //
    // private void Awake()
    // {
    //     DPPool = new List<DamagePopUpScript>();
    //     for (int i = 0; i < initialSize; i++)
    //     {
    //         DPPool.Add(Instantiate(baseDP, transform));
    //     }
    // }
    //
    // private void FixedUpdate()
    // {
    //     if (Time.time - timeNow > 10f)
    //     {
    //         CleanUpPool();
    //         timeNow = Time.time;
    //     }
    // }
    //
    // public override void displayDamage(float dmg, Color colour)
    // {
    //     DamagePopUpScript currentDP = GetNextDP();
    //     currentDP.displayDamage(dmg, colour);
    // }
    //
    //
    //
    // public override void displayCriticalDamage(float dmg)
    // {
    //     DamagePopUpScript currentDP = GetNextDP();
    //     currentDP.displayCriticalDamage(dmg);
    // }
    //
    // DamagePopUpScript GetNextDP()
    // {
    //     int i = 0;
    //     pointer = (pointer+1) % DPPool.Count;
    //
    //     DamagePopUpScript currentDP = DPPool[pointer];
    //     while (i < DPPool.Count && currentDP.checkText())
    //     {
    //         pointer = (pointer + 1) % DPPool.Count;
    //         currentDP = DPPool[pointer];
    //         i++;
    //     }
    //     if (currentDP.checkText())
    //     {
    //         Debug.Log($"{transform.parent} damage pool overflow");
    //         currentDP = Instantiate(baseDP, GetRandomPosition(),Quaternion.identity, transform);
    //         DPPool.Add(currentDP);
    //     }
    //     return currentDP;
    //
    // }
    //
    // void CleanUpPool()
    // {
    //     if (DPPool.Count > initialSize)
    //     {
    //        // print(this + " loading clean up");
    //         int i = 0;
    //         DamagePopUpScript currentDP;
    //         while (i < DPPool.Count && DPPool.Count > initialSize)
    //         {
    //             currentDP = DPPool[i];
    //             i++;
    //             if (!currentDP.checkText())
    //             {
    //                 //print(this + " clearing " + i);
    //                 DPPool.Remove(currentDP);
    //                 Destroy(currentDP.gameObject);
    //                 i--;
    //             }
    //         }
    //     }
    // }
}


