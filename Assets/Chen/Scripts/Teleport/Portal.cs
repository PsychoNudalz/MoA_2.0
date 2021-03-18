using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal portalTarget;

    [SerializeField]
    Transform targetSpawner;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup() {
        targetSpawner = portalTarget.transform.Find("SpawnPoint");
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            Debug.Log("Ohhhhhhhhhhhhhhhhhhhhhhhhh");
            GameObject player = GameObject.FindWithTag("Player");
            player.SetActive(false);
            player.transform.position = targetSpawner.transform.position;
            player.SetActive(true);
        }
    }
}
