using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look : MonoBehaviour
{
    public float rotateSpeed = 500f;

    public Transform CharacterBody;
    public Transform UpDown;


    public float maxRotationDown = 40f;

    float yRotation = 0f;
    private bool look = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;

            float mouseY = Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;
            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -maxRotationDown, maxRotationDown);



        UpDown.transform.localRotation = Quaternion.Euler(yRotation,0f, 0f);

        CharacterBody.Rotate(Vector3.up * mouseX);
    }
}
