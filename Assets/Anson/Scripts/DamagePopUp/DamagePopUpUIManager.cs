using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUpUIManager : MonoBehaviour
{
    public static DamagePopUpUIManager current;

    [SerializeField] List<DamagePopUpUIScript> DPPool;
    [SerializeField] DamagePopUpUIScript baseDP;
    [SerializeField] int pointer = 0;
    [SerializeField] int initialSize = 10;
    [SerializeField] float spawnRange = .2f;
    float timeNow = 0;

    private void Awake()
    {
        current = this;
        DPPool = new List<DamagePopUpUIScript>();
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

    public DamagePopUpUIScript displayDamage(string dmg, Color colour,DamagePopUpScript damagePopUpScript)
    {
        DamagePopUpUIScript currentDP = GetNextDP();
        currentDP.SetText(dmg,colour,damagePopUpScript);
        return currentDP;
    }



    DamagePopUpUIScript GetNextDP()
    {
        int i = 0;
        pointer = (pointer) % DPPool.Count;

        DamagePopUpUIScript currentDP = DPPool[pointer];
        while (i < DPPool.Count && currentDP.gameObject.activeSelf)
        {
            pointer = (pointer + 1) % DPPool.Count;
            currentDP = DPPool[pointer];
            i++;
        }
        if (currentDP.gameObject.activeSelf)
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
            // print(this + " loading clean up");
            int i = 0;
            DamagePopUpUIScript currentDP;
            while (i < DPPool.Count && DPPool.Count > initialSize)
            {
                currentDP = DPPool[i];
                i++;
                if (!currentDP.gameObject.activeSelf)
                {
                    //print(this + " clearing " + i);
                    DPPool.Remove(currentDP);
                    Destroy(currentDP.gameObject);
                    i--;
                }
            }
        }
    }
}
