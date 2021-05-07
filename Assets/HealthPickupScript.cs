using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum HealthPickupType { Large, Small, Regen };
public class HealthPickupScript : MonoBehaviour
{
    [SerializeField] private HealthPickupType pickupType;
    [SerializeField] private float smallPercentageValue;
    [SerializeField] private float largePercentageValue;
    [SerializeField] private float regenDuration;
    PlayerLifeSystemScript playerLifeSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            playerLifeSystem = other.transform.GetComponent<PlayerLifeSystemScript>();
            CollectPickup();
        }
    }

    private void CollectPickup()
    {
        if(playerLifeSystem != null)
        {
            switch (pickupType)
            {
                case HealthPickupType.Small:
                    print(String.Format("Small health pickup : {0}%", smallPercentageValue));
                    playerLifeSystem.healHealth_PercentageMissing(smallPercentageValue);
                    break;
                case HealthPickupType.Large:
                    print(String.Format("Large health pickup : {0}%", largePercentageValue));
                    playerLifeSystem.healHealth_Percentage(largePercentageValue);
                    break;
                case HealthPickupType.Regen:
                    print(String.Format("Regen health pickup : {0} seconds", regenDuration));
                    //Start regen.
                    break;
            }
            GameObject.Destroy(this.gameObject);
        }
    }
}
