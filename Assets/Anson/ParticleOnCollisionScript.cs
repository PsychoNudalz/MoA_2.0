using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleOnCollisionScript : MonoBehaviour
{
    public List<GameObject> spawnItems;
    public float duration;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject currentItem;
        foreach(GameObject item in spawnItems)
        {
            currentItem = Instantiate(item, collision.contacts[0].point, Quaternion.identity);
            Destroy(currentItem, duration);
        }
    }

}
