using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrailControllerScript : MonoBehaviour
{
    [SerializeField]
    private LineRenderer baseTrail;

    [SerializeField]
    private List<LineRenderer> bulletTrails;

    [SerializeField]
    private float trailTime = 0.5f;
    float trailDistance = 100f;

    private int numberOfTrails;
    private int trailPointer = 0;

    [ContextMenu("Start")]
    private void Start()
    {
        if (!baseTrail)
        {
            baseTrail = GetComponentInChildren<LineRenderer>();
            
        }
        
    }

    public void InitialiseTrails(int i, float timeBetweenShots)
    {
        baseTrail.material.SetInt("_ElementEnum",i);
        baseTrail.material.SetFloat("_Duration",trailTime);
        
        numberOfTrails = Mathf.RoundToInt((trailTime/timeBetweenShots)*2f+1);
        bulletTrails.Add(baseTrail);
        for (int j = 0; j < numberOfTrails; j++)
        {
            CreateNewTrail();
        }

        foreach (LineRenderer lineRenderer in bulletTrails)
        {
            lineRenderer.gameObject.SetActive(false);
        }
    }

    private LineRenderer CreateNewTrail()
    {
        LineRenderer newLine = Instantiate(baseTrail, transform).GetComponent<LineRenderer>();
        bulletTrails.Add(newLine);
        return newLine;
    }

    public void PlayTrail(RaycastHit raycastHit)
    {
        LineRenderer current = GetNextTrail();
        current.SetPosition(0,transform.position);
        current.SetPosition(1,raycastHit.point);
    }
    public void PlayTrail(Vector3 dir)
    {
        LineRenderer current = GetNextTrail();
        current.SetPosition(0,transform.position);
        current.SetPosition(1,transform.position+(dir*trailDistance));
    }

    public void PlayTrail(List<RaycastHit> raycastHits)
    {
        foreach (RaycastHit raycastHit in raycastHits)
        {
            PlayTrail(raycastHit);
        }
    }

    public void PlayTrail(List<Vector3> fireDirs)
    {
        foreach (Vector3 fireDir in fireDirs)
        {
            PlayTrail(fireDir);
        }
    }


    /// <summary>
        /// Object Pool Trails
        /// </summary>
        /// <returns></returns>
    private LineRenderer GetNextTrail()
    {
        LineRenderer current = bulletTrails[trailPointer % bulletTrails.Count];
        trailPointer = (trailPointer + 1) % bulletTrails.Count;
        if (current.gameObject.activeSelf)
        {
            current = CreateNewTrail();
        }
        current.gameObject.SetActive(true);
        current.material.SetFloat("_EndingTime",Time.time);

        StartCoroutine(DelayDeactiveTrail(current.gameObject));
        return current;
    }

    IEnumerator DelayDeactiveTrail(GameObject g)
    {
        yield return new WaitForSeconds(trailTime);
        g.SetActive(false);
    }
}
