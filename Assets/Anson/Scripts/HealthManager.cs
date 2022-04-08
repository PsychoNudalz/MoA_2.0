using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField]
    private GameObject smallHealth;
    [SerializeField]
    private GameObject bigHealth;
    [SerializeField]
    private GameObject regenHealth;

    public static HealthManager current;

    public GameObject SmallHealth => smallHealth;

    public GameObject BigHealth => bigHealth;

    public GameObject RegenHealth => regenHealth;

    // Start is called before the first frame update
    void Awake()
    {
        if (!current)
        {
            current = this;
        }
    }

    public static void SpawnHealth(HealthPickupType healthPickupType, Vector3 location)
    {
        GameObject spawnObject = null;
        switch (healthPickupType)
        {
            case HealthPickupType.Large:
                spawnObject = current.bigHealth;
                break;
            case HealthPickupType.Small:
                spawnObject = current.smallHealth;

                break;
            case HealthPickupType.Regen:
                spawnObject = current.regenHealth;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(healthPickupType), healthPickupType, null);
        }

        spawnObject = Instantiate(spawnObject, location, quaternion.identity);
        spawnObject.name.Replace("(Clone)", "");
    }
   
}
