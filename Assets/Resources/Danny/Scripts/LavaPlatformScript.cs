using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPlatformScript : MonoBehaviour
{
    [SerializeField] GameObject lavaStart, lavaEnd;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 targetPosition;
    private Vector3 startPosition;

    private void Awake()
    {
        targetPosition = new Vector3(lavaEnd.transform.position.x, transform.position.y, transform.position.z);
        startPosition = new Vector3(lavaStart.transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(targetPosition,transform.position) <= 1f)
        {
            transform.position = startPosition;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,targetPosition, (moveSpeed / 100));
            if(rotationSpeed >= 0)
            {
                transform.Rotate(new Vector3(0, 0, Random.Range(-0.1f,rotationSpeed)));
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, Random.Range(rotationSpeed, 0.1f)));
            }
        }
    }
}
