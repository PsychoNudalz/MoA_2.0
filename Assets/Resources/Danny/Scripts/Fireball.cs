using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float lifeTimeDuration = 5f;
    private Vector3 target;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 playerDirection = player.transform.position - transform.position;
        Vector3 shootDirection = (new Vector3(playerDirection.x, 0f, playerDirection.z));
        //Debug.DrawRay(transform.position, shootDirection, Color.red);
        target = transform.position + shootDirection * 2f;
        //Debug.DrawRay(transform.position, target * 10f, Color.red);
        GameObject.Destroy(this, lifeTimeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                print("player hit");
                GameObject.Destroy(this);
                break;
            default:
                GameObject.Destroy(this);
                break;
        }
    }
}
