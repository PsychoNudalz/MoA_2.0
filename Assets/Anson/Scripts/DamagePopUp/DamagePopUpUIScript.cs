using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Camera = UnityEngine.Camera;

public class DamagePopUpUIScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private DamagePopUpScript pairedDamage;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (pairedDamage)
        {
            if (pairedDamage.checkText())
            {
                UpdateText(pairedDamage.transform.position);

            }
            else
            {
                EndUI();
            }
        }
        else
        {
            EndUI();
        }
    }

    public void SetText(string s, Color c, DamagePopUpScript damagePopUpScript)
    {
        if (!cam)
        {
            cam = Camera.main;
        }
        text.text = s;
        text.color = c;
        pairedDamage = damagePopUpScript;
        gameObject.SetActive(true);
        UpdateText(pairedDamage.transform.position);

    }

    public void UpdateText(Vector3 worldPosistion)
    {
        transform.position = cam.WorldToScreenPoint(worldPosistion);
        if (!text.text.Equals(pairedDamage.displayText))
        {
            EndUI();
            Debug.LogWarning($"Text mismatch from {this.name} & {pairedDamage.name}");
        }
    }

    public void EndUI()
    {
        text.text = "";
        gameObject.SetActive(false);
    }
}
