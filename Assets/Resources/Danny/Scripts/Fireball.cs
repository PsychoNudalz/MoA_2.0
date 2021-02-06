using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float lifeTimeDuration = 10f;
    private Vector3 target;
    //private GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        /*
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerDirection = player.transform.position - transform.position;
        Vector3 shootDirection = (new Vector3(playerDirection.x, 0f, playerDirection.z));
        //Debug.DrawRay(transform.position, shootDirection, Color.red);
        target = transform.position + shootDirection * 5f;
        //Debug.DrawRay(transform.position, target * 10f, Color.red);
        */
        GameObject.Destroy(this.gameObject, lifeTimeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
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
        target = aimTarget;
        Debug.DrawLine(transform.position, target, Color.red, 5f);
    }
}
