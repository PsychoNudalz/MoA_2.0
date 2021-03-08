﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TargetMaterialHandlerScript : MonoBehaviour
{
    [Header("Target Material")]
    [SerializeField] Renderer render;
    [SerializeField] Material material;
    [Header("Decay State")]
    [SerializeField] float currentRatio;
    [SerializeField] float decayTime = 1;
    [Header("Shock Effect")]
    [SerializeField] GameObject shockEffect;
    VisualEffect currentShock_vfx;
    [SerializeField] Transform EffectsParent;
    List<VisualEffect> allShockList = new List<VisualEffect>();
    int allShockListPTR = 0;
    List<VisualEffect> shockList = new List<VisualEffect>();
    List<Transform> shockTargets = new List<Transform>();
    // Start is called before the first frame update
    void Awake()
    {
        material = render.materials[0];
        decayTime = material.GetFloat("_DecayTime");
        ExpandAllShock();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentRatio > 0)
        {
            currentRatio -= (1 / decayTime) * Time.deltaTime;
            material.SetFloat("_Ratio", currentRatio);
        }
    }

    public void StartDecay()
    {
        currentRatio = 1;
    }

    public void SetFire(bool b)
    {
        if (b)
        {
            material.SetInt("_SetFire", 1);

        }
        else
        {
            material.SetInt("_SetFire", 0);

        }
    }

    public void SetIce(bool b)
    {
        if (b)
        {
            material.SetInt("_SetIce", 1);

        }
        else
        {
            material.SetInt("_SetIce", 0);

        }
    }

    public void SetShock(bool b, Transform shockTarget = null)
    {
        if (b)
        {
            currentShock_vfx = GetNextShock();
            currentShock_vfx.gameObject.SetActive(true);
            currentShock_vfx.SendEvent("ShockSelf");
            //Destroy(currentShock_vfx.gameObject, currentShock_vfx.GetFloat("Lifetime")*1.5f);

            //shockEffect.SetActive(true);
            if (shockTarget != null)
            {
                shockList.Add(currentShock_vfx);
                shockTargets.Add(shockTarget.transform);
                currentShock_vfx.SetFloat("ChainLength", (shockTarget.position - transform.position).magnitude);
                currentShock_vfx.transform.forward = shockTarget.position - transform.position;
                currentShock_vfx.SendEvent("ShockChain");
            }

        }
    }

    public void UpdateShock()
    {
        for (int i = 0; i < shockList.Count && i < shockTargets.Count; i++)
        {
            if (shockTargets[i] != null && shockList[i] != null)
            {
                shockList[i].SetFloat("ChainLength", (shockTargets[i].position - transform.position).magnitude);
                shockList[i].transform.forward = shockTargets[i].position - transform.position;
                shockList[i].SendEvent("ShockChain");
            }
        }
    }



    public void ResetShockList()
    {

        shockList = new List<VisualEffect>();
        shockTargets = new List<Transform>();
    }

    VisualEffect GetNextShock()
    {
        int PTR = (allShockListPTR + 1) % allShockList.Count;
        VisualEffect current = allShockList[PTR];
        while (PTR != allShockListPTR && current.gameObject.activeSelf)
        {
            PTR = (PTR + 1) % allShockList.Count;
            current = allShockList[PTR];
        }
        if (PTR == allShockListPTR)
        {
            ExpandAllShock();
            allShockListPTR++;
        }
        else
        {
            allShockListPTR = PTR;

        }

        current = allShockList[allShockListPTR];
        current.gameObject.SetActive(true);
        StartCoroutine(DisableShockDelay(current));
        return current;
    }

    void ExpandAllShock()
    {
        if (allShockList.Count != 0)
        {
        allShockListPTR = (allShockList.Count - 1) % allShockList.Count;

        }
        VisualEffect current;
        for (int i = 0; i < 5; i++)
        {
            current = Instantiate(shockEffect, shockEffect.transform.position, Quaternion.identity, EffectsParent).GetComponent<VisualEffect>();
            allShockList.Add(current);
            current.gameObject.SetActive(false);
        }
    }
    IEnumerator DisableShockDelay(VisualEffect v)
    {
        yield return new WaitForSeconds(v.GetFloat("Lifetime") * 1.5f);
        v.gameObject.SetActive(false);

    }
}
