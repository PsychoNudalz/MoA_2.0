using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTriggerDetectionScript : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] ProjectileScript projectileScript;
    [SerializeField] SphereCollider triggerArea;
    [SerializeField] float radius;
    [SerializeField] List<string> tagList;
    [SerializeField] bool canOverrideTarget;


    private void Awake()
    {
        if (triggerArea == null)
        {
            triggerArea = GetComponent<SphereCollider>();
        }
        if (triggerArea != null)
        {
            triggerArea.radius = radius;
        }

        if (projectileScript == null)
        {
            projectileScript = GetComponentInParent<ProjectileScript>();
        }
        if (projectileScript != null)
        {
            tagList = projectileScript.TagList;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tagList.Contains(other.tag) && !other.tag.Equals("Enviorment"))
        {
            if (canOverrideTarget || target == null)
            {
                target = other.gameObject;
                projectileScript.SetHoming(target.transform);
            }
        }
    }

}
