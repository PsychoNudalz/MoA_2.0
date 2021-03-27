using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryScript : MonoBehaviour
{
    [SerializeField] AIGunDamageScript aIGunDamageScript;
    [SerializeField] float fireInterval;

    private void Awake()
    {
        //aIGunDamageScript = GetComponent<AIGunDamageScript>();
    }

    private void FixedUpdate()
    {
        aIGunDamageScript.Fire(true);
    }
}
