﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TargetMaterialHandlerScript : MonoBehaviour
{
    [Header("Target Material")]
    [SerializeField] MeshRenderer ms;
    [SerializeField] Material material;
    [Header("Decay State")]
    [SerializeField] float currentRatio;
    [SerializeField] float decayTime;
    [Header("Shock Effect")]
    [SerializeField] GameObject shockEffect;
    VisualEffect currentShock_vfx;
    List<VisualEffect> shockList = new List<VisualEffect>();
    // Start is called before the first frame update
    void Awake()
    {
        material = ms.material;
        decayTime = material.GetFloat("_DecayTime");
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

    public void SetShock(bool b, Transform shockTarget = null)
    {
        if (b)
        {
            currentShock_vfx = Instantiate(shockEffect, shockEffect.transform.position, Quaternion.identity, transform).GetComponent<VisualEffect>();
            currentShock_vfx.gameObject.SetActive(true);
            currentShock_vfx.SendEvent("ShockSelf");
            Destroy(currentShock_vfx.gameObject, currentShock_vfx.GetFloat("Lifetime") * 1.1f);

            //shockEffect.SetActive(true);
            if (shockTarget != null)
            {
                shockList.Add(currentShock_vfx);
                currentShock_vfx.SetFloat("ChainLength", (shockTarget.position - transform.position).magnitude);
                currentShock_vfx.transform.forward = shockTarget.position - transform.position;
                currentShock_vfx.SendEvent("ShockChain");
            }

        }
    }



    public void ResetShockList()
    {
        shockList = new List<VisualEffect>();
    }
}
