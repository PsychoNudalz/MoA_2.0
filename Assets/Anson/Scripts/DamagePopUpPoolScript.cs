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
    float timeNow = 0;

    private void Awake()
    {
        DPPool = new List<DamagePopScript>();
        for (int i = 0; i < initialSize; i++)
        {
            DPPool.Add(Instantiate(baseDP, transform));
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - timeNow > 10f)
        {
            CleanUpPool();
            timeNow = Time.time;
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
        pointer = (pointer) % DPPool.Count;

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
            DPPool.Add(currentDP);
        }
        return currentDP;

    }

    void CleanUpPool()
    {
        if (DPPool.Count > initialSize)
        {
            print(this + " loading clean up");
            int i = 0;
            DamagePopScript currentDP;
            while (i < DPPool.Count && DPPool.Count > initialSize)
            {
                currentDP = DPPool[i];
                i++;
                if (!currentDP.checkText())
                {
                    print(this + " clearing " + i);
                    DPPool.Remove(currentDP);
                    Destroy(currentDP.gameObject);
                    i--;
                }
            }
        }
    }
}
