using System.Collections;
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
    [SerializeField] VisualEffect shock_vfx;
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
        shockEffect.SetActive(false);
        if (b)
        {
            shockEffect.SetActive(true);
            if (shockTarget == null)
            {
                shock_vfx.SetFloat("ChainLength", 0);
            }
            else
            {
                shock_vfx.SetFloat("ChainLength", (shockTarget.position - transform.position).magnitude);
                shockEffect.transform.forward = shockTarget.position - transform.position;
            }

        }
    }
}
