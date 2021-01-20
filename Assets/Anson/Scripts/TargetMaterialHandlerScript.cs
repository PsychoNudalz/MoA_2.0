using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMaterialHandlerScript : MonoBehaviour
{
    [SerializeField] MeshRenderer ms;
    [SerializeField] Material material;
    [SerializeField] float currentRatio;
    [SerializeField] float decayTime;
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
}
