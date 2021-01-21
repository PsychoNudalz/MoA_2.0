using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpPoolScript : DamagePopScript
{
    [SerializeField] List<DamagePopScript> DPPool;
    [SerializeField] DamagePopScript baseDP;
    [SerializeField] int pointer = 0;
    [SerializeField] int initialSize = 10;
    [SerializeField] float spawnRange = .2f;

    private void Awake()
    {
        DPPool = new List<DamagePopScript>();
        for (int i = 0; i < initialSize; i++)
        {
            DPPool.Add(Instantiate(baseDP, transform));
        }
    }

    public override void displayDamage(float dmg, Color colour)
    {
        DamagePopScript currentDP = GetNextDP();
        currentDP.displayDamage(dmg, colour);
        currentDP.transform.localPosition = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
    }

    public override void displayCriticalDamage(float dmg)
    {
        DamagePopScript currentDP = GetNextDP();
        currentDP.displayCriticalDamage(dmg);
        currentDP.transform.localPosition = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
    }

    DamagePopScript GetNextDP()
    {
        int i = 0;
        DamagePopScript currentDP = DPPool[pointer];
        while (i < DPPool.Count && currentDP.checkText())
        {
            pointer = (pointer + 1) % DPPool.Count;
            currentDP = DPPool[pointer];
            i++;
        }
        if (currentDP.checkText())
        {
            currentDP = Instantiate(baseDP, transform);
        }
        return currentDP;

    }
}
