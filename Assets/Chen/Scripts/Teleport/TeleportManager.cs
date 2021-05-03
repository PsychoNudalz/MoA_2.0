using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public List<Portal> portals = new List<Portal>();
    public Portal start, end;

    void Start()
    {
        ShuffleList(portals);
        Portal prev = start;
        int i = 0;
        foreach (Portal pt in portals)
        {
            prev.portalTarget = pt;
            if (pt.CurrentRoomEnemySystem != null)
            {
                prev.Setup(pt.CurrentRoomEnemySystem,i);
            }
            prev = pt;
            i++;
        }
        prev.portalTarget = end;
        prev.Setup(end.CurrentRoomEnemySystem,i);
        //end.GetComponent<BoxCollider>().enabled = false;
    }

    void ShuffleList(List<Portal> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;

        for (int i = n - 1; i > 1; i--)
        {
            int rng = random.Next(i + 1);
            Portal tmp = list[rng];
            list[rng] = list[i];
            list[i] = tmp;
        }
    }
}