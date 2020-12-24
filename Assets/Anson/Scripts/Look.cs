using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    public float rotateSpeed = 500f;

    public Transform CharacterBody;
    public Transform UpDown;
    public Vector2 mouseValue;


    public float maxRotationDown = 40f;

    float yRotation = 0f;
    private bool look = false;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = mouseValue.x * rotateSpeed * Time.deltaTime;

        float mouseY = mouseValue.y * rotateSpeed * Time.deltaTime;
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -maxRotationDown, maxRotationDown);



        UpDown.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        CharacterBody.Rotate(Vector3.up * mouseX);
    }

    public void LookMouse(InputAction.CallbackContext callbackContext)
    {
        mouseValue = callbackContext.ReadValue<Vector2>();
    }
}
