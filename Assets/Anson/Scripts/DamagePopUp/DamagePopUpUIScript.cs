using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Camera = UnityEngine.Camera;

public class DamagePopUpUIScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 worldPosition;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Animation startAnimation;

    private Camera cam;
    

    private void Awake()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        if (gameObject.activeSelf)
        {
            UpdateText();
        }
        else
        {
            EndUI();
        }
    }

    public void SetText(string s, Color c, Vector3 worldPos)
    {
        if (!cam)
        {
            cam = Camera.main;
        }

        text.text = s;
        text.color = c;
        gameObject.SetActive(true);
        worldPosition = worldPos;
        UpdateText();
    }



    public void UpdateText()
    {
        transform.position = cam.WorldToScreenPoint(worldPosition);
    }

    public void EndUI()
    {
        text.text = "";
        gameObject.SetActive(false);
    }
    
    private void OnEnable()
    {
        startAnimation.Play();
    }
}