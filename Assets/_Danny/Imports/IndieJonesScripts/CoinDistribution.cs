using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class CoinDistribution : MonoBehaviour
{
    /*
    [SerializeField]
    private GameObject coinPrefab;
   
    [SerializeField]
    private int numberOfCoins;

    //private NavMeshHit myNavHit;
    //private float maxDist = 0.8f;
    //private Vector3 initialPosition;
    Vector3 positionToTry;

    private void Awake()
    {
        for(int i = 0; i < numberOfCoins; i++)
        {
            bool isValid = false;
            while (!isValid)
            {
                isValid = true;
                positionToTry = GetRandomLocation();
                Collider[] collisions = Physics.OverlapSphere(positionToTry, 0.5f);
                foreach (Collider collider in collisions)
                {
                    if (collider.CompareTag("Coin"))
                    {
                        isValid = false;
                        break;
                    }
                }
            }
            
            GameObject coin = Instantiate(coinPrefab, positionToTry, Quaternion.identity);
            coin.transform.parent = this.transform;
        }

        Debug.Log(FindObjectsOfType<Coin>().Length + " Coins Created");
    }

    Vector3 GetRandomLocation()
    {
        NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();

        // Pick the first indice of a random triangle in the nav mesh
        int t = Random.Range(0, navMeshData.indices.Length - 3);

        // Select a random point on it
        Vector3 point = Vector3.Lerp(navMeshData.vertices[navMeshData.indices[t]], navMeshData.vertices[navMeshData.indices[t + 1]], Random.value);
        Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value);

        return point;
    }
}
    /*
    void Awake()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            bool validPosition = false;
            while (!validPosition)
            {
                positionToTry = initialPosition - new Vector3(Random.RandomRange(0 - levelSize, levelSize), 0.0f, Random.RandomRange(-levelSize, levelSize));
                positionToTry = raycastDown(positionToTry);

                if (NavMesh.SamplePosition(positionToTry, out myNavHit, maxDist, 1 << NavMesh.GetAreaFromName("Walkable")))
                {
                    validPosition = true;
                }

                if (validPosition)
                {
                    Collider[] collisions = Physics.OverlapSphere(positionToTry, 2f);

                    foreach (Collider collider in collisions)
                    {
                        if (collider.CompareTag("Coin"))
                        {
                            validPosition = false;
                            break;
                        }
                    }
                }
            }
            GameObject coin = Instantiate(coinPrefab, myNavHit.position, Quaternion.identity);
            coin.transform.parent = this.transform;
        }
        Debug.Log(FindObjectsOfType<Coin>().Length + " Coins Created");
    }


    private Vector3 raycastDown(Vector3 positionToCastFrom)
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(positionToCastFrom, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.transform.position;
            Debug.Log("Did Hit");
        }
        else
        {
            return new Vector3(-1000, -1000, -1000);
            Debug.Log("Did not Hit");
        }
    }
}        /*
        for(int i = 0; i < numberOfCoins; i++)
        {
            bool validPosition = false;
            while (!validPosition)
            {
                Vector3 positionToTry = initialPosition - new Vector3(Random.RandomRange(0 - levelSize, levelSize), 0.0f, Random.RandomRange(-levelSize, levelSize));
                Collider[] collisions = Physics.OverlapSphere(positionToTry, 2f);

                validPosition = true;

                foreach(Collider collider in collisions)
                {
                    if (collider.CompareTag("Coin"))
                    {

                        validPosition = false;
                        break;
                    }
                }

                if (NavMesh.SamplePosition(positionToTry, out myNavHit, maxDist, 1 << NavMesh.GetAreaFromName("Walkable")) && validPosition)
                {
                    GameObject coin = Instantiate(myPrefab, myNavHit.position, Quaternion.identity);
                    coin.transform.parent = this.transform;
                }
                else
                {
                    validPosition = false;
                }
            }
        }
        Debug.Log( FindObjectsOfType <Coin> ().Length + " Coins Created");
    */
    }
        
   
