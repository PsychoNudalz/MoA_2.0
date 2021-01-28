using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Enemy {Chicken, Skeleton };
public class EnemySpawner : MonoBehaviour
{
    /*
    [SerializeField]
    private GameObject chickenPrefab;
    [SerializeField]
    private GameObject skeletonPrefab;

    public void SpawnEnemy(Enemy enemyType, Vector3 position, float delay, Transform waypoints)
    {
        if(enemyType.Equals(Enemy.Chicken)){
            StartCoroutine(RespawnChicken(position, delay, waypoints));
        }
        else if (enemyType.Equals(Enemy.Skeleton))
        {
            StartCoroutine(RespawnSkeleton(position, delay, waypoints));
        }
        
    }

    public IEnumerator RespawnChicken(Vector3 position, float delay, Transform waypoints)
    {
        //Debug.Log(string.Format("Respawning Chicken at position {0}, Delay {1}, Waypoints - {2}", position.ToString(), delay, waypoints.ToString()));
        yield return new WaitForSecondsRealtime(delay);
        GameObject respawn = Instantiate(chickenPrefab, position, Quaternion.identity);
        ChickenAgent agent = respawn.GetComponent<ChickenAgent>();
        agent.waypointsToFollow = waypoints;
        agent.enabled = true;
        agent.ResetChicken();
        //Debug.Log("Spawned Chicken");
    }

    public IEnumerator RespawnSkeleton(Vector3 position, float delay, Transform waypoints)
    {
        //Debug.Log(string.Format("Respawning Skeleton at position {0}, Delay {1}, Waypoints - {2}", position.ToString(), delay, waypoints.ToString()));
        yield return new WaitForSecondsRealtime(delay);
        GameObject respawn = Instantiate(skeletonPrefab, position, Quaternion.identity);
        SkeletonAgent agent = respawn.GetComponent<SkeletonAgent>();
        agent.waypointsToFollow = waypoints;
        agent.enabled = true;
        agent.ResetSkeleton();
        //Debug.Log("Spawned Skeleton");
    }
    */
}
