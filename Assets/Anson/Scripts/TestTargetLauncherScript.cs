using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTargetLauncherScript : MonoBehaviour
{
    [SerializeField] Rigidbody launchedOB;
    [SerializeField] float launchStrength = 1000f;
    [SerializeField] float lauchTorque = 100f;
    [SerializeField] float spawnInterval;
    [SerializeField] float timeNow;



    // Update is called once per frame
    void Update()
    {
        timeNow -= Time.deltaTime;
        if (timeNow < 0)
        {
            timeNow = spawnInterval;
            Rigidbody rb = Instantiate(launchedOB.gameObject, transform.position, transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(launchStrength*rb.mass*transform.up);
            rb.AddTorque(launchStrength * new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)));
        }
    }

}
