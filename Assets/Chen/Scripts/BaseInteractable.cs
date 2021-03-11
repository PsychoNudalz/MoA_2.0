using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseInteractable : MonoBehaviour
{
    public GameObject toolTip;
    public GameObject linkedUI;
    Keyboard keyboard;
    
    private void Awake() {
        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            toolTip.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            toolTip.SetActive(false);
            linkedUI.SetActive(false);
            UIExitCallBack();
        }
    }

    private void Update() {
        if (keyboard.eKey.wasReleasedThisFrame && toolTip.activeSelf && !linkedUI.activeSelf) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            linkedUI.SetActive(true);
        }
    }

    public void UIExitCallBack() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
