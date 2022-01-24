using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private float meleeDamage;

    [SerializeField]
    private float meleeRange;

    [SerializeField]
    private AnimationCurve damageCurve;

    [SerializeField]
    private ElementTypes damageElement = ElementTypes.PHYSICAL;

    [SerializeField]
    private float cooldownTime = 3f;

    [SerializeField]
    float cooldownTime_Now = 0f;


    [Header("Components")]
    [SerializeField]
    private SphereCastDamageScript sphereCastDamageScript;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldownTime_Now >= 0)
        {
            cooldownTime_Now -= Time.deltaTime;
        }
    }

    public void Melee()
    {
        print("Melee");
        cooldownTime_Now = cooldownTime;
        sphereCastDamageScript.SphereCastDamageArea(meleeDamage, meleeRange, damageCurve, 1, damageElement, true);
    }

    public bool CanMelee()
    {
        return cooldownTime_Now <= 0;
    }
}