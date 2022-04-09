using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTriggerDetectionScript : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] ProjectileScript projectileScript;
    [SerializeField] SphereCollider triggerArea;
    [SerializeField] float radius;
    [SerializeField] float deadZone;
    [SerializeField] List<string> tagList;
    [SerializeField] List<string> ignoreTagList;
    [SerializeField] bool canOverrideTarget;
    [SerializeField] float checkRate = 0.5f;
    float lastCheck;


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
            tagList = new List<string>(projectileScript.TagList);
        }

        foreach (string s in ignoreTagList)
        {
            if (tagList.Contains(s))
            {
                tagList.Remove(s);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tagList.Contains(other.tag) )
        {
            if (canOverrideTarget || target == null)
            {
                target = other.gameObject;
                if(target.TryGetComponent<LifeSystemScript>(out LifeSystemScript ls))
                {
                    projectileScript.SetHoming(ls.GetCentreOfMass());
                }
                else
                {
                projectileScript.SetHoming(target.transform);

                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Time.time-lastCheck > checkRate)
        {
            lastCheck = Time.time;

            if (target!=null&&(target.transform.position - transform.position).magnitude < deadZone)
            {
                projectileScript.SetHoming(null);

            }
        }
    }

}
