using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseInteractable : InteractableScript
{
    public GameObject toolTip;
    public GameObject linkedUI;
    Keyboard keyboard;

    private void Awake()
    {
        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            toolTip.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            toolTip.SetActive(false);
            if (linkedUI != null)
            {
                linkedUI.SetActive(false);
            }
            UIExitCallBack();
        }
    }

    private void Update()
    {
        if (keyboard.eKey.wasReleasedThisFrame && toolTip.activeSelf && linkedUI != null && !linkedUI.activeSelf)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            if (linkedUI != null)
            {
                linkedUI.SetActive(true);
            }
        }
    }

    public void UIExitCallBack()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
