using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaScript : MonoBehaviour
{
    //player object
    PlayerMasterScript player;

    private void Awake()
    {
        //find player
        player = FindObjectOfType<PlayerMasterScript>();
    }
    private void OnTriggerEnter(Collider other)
    {
        // if player enters box collider trigger, then kill the player
        if (other.tag.Equals("Player")) {
            print("Kill Player");
        }
    }

}
