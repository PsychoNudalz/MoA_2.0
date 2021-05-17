using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGunDamageScript : GunDamageScript
{
    // Start is called before the first frame update
    void Start()
    {
        isAI = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBehaviour();
    }

    protected override void UpdateBehaviour()
    {
        base.UpdateBehaviour();
        if (!isFiring && currentRecoilTime >0)
        {
            CorrectRecoil();

        }
    }
}
