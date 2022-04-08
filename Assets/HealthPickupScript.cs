using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HealthPickupType { Large, Small, Regen };
public class HealthPickupScript : MonoBehaviour
{
    [SerializeField] private HealthPickupType pickupType;
    [SerializeField] private float smallPercentageValue;
    [SerializeField] private float largePercentageValue;
    [SerializeField] private float regenDuration;
    PlayerLifeSystemScript playerLifeSystem;

    [Header("Text")]
    [SerializeField] Transform text;
    [SerializeField] Camera camera;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag.Equals("Player"))
        {
            playerLifeSystem = other.transform.GetComponent<PlayerLifeSystemScript>();
            CollectPickup();
        }
    }

    private void FixedUpdate()
    {
        RotateTextToCamera();
    }

    private void CollectPickup()
    {
        if(playerLifeSystem != null)
        {
            switch (pickupType)
            {
                case HealthPickupType.Small:
                    print($"Small health pickup : {smallPercentageValue}%");
                    playerLifeSystem.healHealth_PercentageMissing(smallPercentageValue);
                    break;
                case HealthPickupType.Large:
                    print($"Large health pickup : {largePercentageValue}%");
                    playerLifeSystem.healHealth_Percentage(largePercentageValue);
                    break;
                case HealthPickupType.Regen:
                    print($"Regen health pickup : {regenDuration} seconds");
                    //Start regen.
                    break;
            }
            GameObject.Destroy(this.gameObject);
        }
    }

    void RotateTextToCamera()
    {
        if (camera == null)
        {
            camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        Vector3 dir = camera.transform.position - transform.position;
        transform.forward = -dir;
    }
}
