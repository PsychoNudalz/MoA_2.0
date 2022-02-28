using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private Sprite currentCrosshairSprite;

    [Range(0f, 80f)]
    [SerializeField]
    private float degree = 0f;

    [SerializeField]
    private float adjacentSide = 1f;

    [SerializeField]
    private List<RectTransform> crosshairPoints;


    [Header("Components")]
    [SerializeField]
    private PlayerGunDamageScript playerGunDamageScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerGunDamageScript)
        {
            if (Math.Abs(degree - playerGunDamageScript.RecoilDegree) > .01f)
            {
                degree = playerGunDamageScript.RecoilDegree;
                UpdateCrosshair();
            }
        }
        else
        {
            UpdateCrosshair();
        }
    }

    void UpdateCrosshair()
    {
        float distance = CalculateOpposite();
        Vector3 crosshairPointPosition = new Vector3(0, distance);
        int i = 0;
        float rotateAmount = 360f / crosshairPoints.Count;
        foreach (RectTransform crosshairPoint in crosshairPoints)
        {
            crosshairPoint.localPosition = crosshairPointPosition;
            crosshairPointPosition = Quaternion.AngleAxis(rotateAmount,Vector3.forward)* crosshairPointPosition;
        }
    }

    private float CalculateOpposite()
    {
        degree = Mathf.Clamp(degree, 0, 80f);
        return Mathf.Tan(Mathf.Deg2Rad * degree) * adjacentSide;
    }

    public void SetCrosshair(bool b)
    {
        if (b)
        {
            foreach (RectTransform rt in crosshairPoints)
            {
                rt.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (RectTransform rt in crosshairPoints)
            {
                rt.gameObject.SetActive(false);
            }
        }
    }


}
