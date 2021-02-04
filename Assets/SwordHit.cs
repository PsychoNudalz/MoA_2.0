using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHit : MonoBehaviour
{
   [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("player")) { 
        
        }
    }
}
