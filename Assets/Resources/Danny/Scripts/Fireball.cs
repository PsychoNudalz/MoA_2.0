using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float lifeTimeDuration = 10f;
    private Vector3 target;
    

    // Start is called before the first frame update
    void Start()
    {
        //Set object to destroy if no collision is made within lifetime
        GameObject.Destroy(this.gameObject, lifeTimeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        /*
         * Move towards target
         */
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        /*
         * If collision with player do damage and destroy itself, if other object just destroy.
         */
        print("collision - " + other.gameObject.ToString());
        switch (other.gameObject.tag)
        {
            case "Player":
                print("player hit");
                GameObject.Destroy(this.gameObject);
                break;
            default:
                GameObject.Destroy(this.gameObject);
                break;
        }
    }

    internal void SetTarget(Vector3 aimTarget)
    {
        /*
         * Set target to move towards
         */
        target = aimTarget;
    }
}
