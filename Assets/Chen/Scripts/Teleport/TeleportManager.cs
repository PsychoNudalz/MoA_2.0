using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    public List<Portal> portals = new List<Portal>(); // List of non-boss room portals
    public List<Portal> bossPortals = new List<Portal>(); // list of boss room portals
    public Portal start;
     Portal end; // Deprecated
    public int percentageHealthReduced = 10;

    void Start()
    {
        ShuffleList(portals);
        //ShuffleList(bossPortals);
        foreach (Portal pt in bossPortals) {
            pt.isBoss = true;
            pt.percentageHealthReduced = percentageHealthReduced;
        }

        if (bossPortals.Count != 0)
        {
            end = bossPortals[bossPortals.Count - 1];
            bossPortals.RemoveAt(bossPortals.Count - 1);
            for (int j = 1; j <= portals.Count / 3; j++) {
                if (bossPortals.Count >= j) portals.Insert(j*3 - 1, bossPortals[j - 1]);
            }
        }

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

        if (end)
        {
            prev.portalTarget = end;
            prev.Setup(end.CurrentRoomEnemySystem,i);
            //end.GetComponent<BoxCollider>().enabled = false;
            end.isWinning = true;
        }

        //Anson: incease player total run
        FindObjectOfType<PlayerMasterScript>().IncreamentRun();
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