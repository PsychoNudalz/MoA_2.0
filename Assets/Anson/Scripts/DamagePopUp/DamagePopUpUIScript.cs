using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Camera = UnityEngine.Camera;

public class DamagePopUpUIScript : MonoBehaviour
{
    [SerializeField]
    private Vector3 worldPosition;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Animation startAnimation;

    [SerializeField]
    private Vector2 screenSize;

    [SerializeField]
    private float margin;

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

    public void Initialise(Vector2 screenSize, float margin)
    {
        this.screenSize = screenSize;
        this.margin = margin;
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
        if (Vector3.Dot(cam.transform.forward, (worldPosition - cam.transform.position).normalized) < 0)
        {
            EndUI();
        }
        Vector2 temp = new Vector2();
        temp.x = Mathf.Clamp(transform.position.x, screenSize.x * margin, screenSize.x * (1 - margin));
        temp.y = Mathf.Clamp(transform.position.y, screenSize.y * margin, screenSize.y * (1 - margin));
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