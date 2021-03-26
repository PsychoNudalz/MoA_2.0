using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    public float rotateSpeed = 500f; //MouseSense
    [SerializeField] float rotateSpeed_Current;

    public Transform CharacterBody;
    public Transform UpDown;
    public Vector2 mouseValue;


    public float maxRotationDown = 40f;

    [SerializeField] float yRotation = 0f;
    private bool look = false;


    [Header("ADS transition")]
    [SerializeField] float FOV = 0f;
    [SerializeField] AnimationCurve aimCurve;
    [SerializeField] private float timeNow_Aim = 0f;
    private bool isADS = false;
    private float currentMult = 1f;

    private Camera camera;

    public float YRotation { get => yRotation; set => yRotation = value; }
    //[SerializeField] bool disableControl = false;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotateSpeed_Current = rotateSpeed;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        FOV = camera.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        MoveLook();
        AdjustAim();
    }

    public void LookMouse(InputAction.CallbackContext callbackContext)
    {
        mouseValue = callbackContext.ReadValue<Vector2>();
    }

    public void MoveLook()
    {
        float mouseX = mouseValue.x * rotateSpeed_Current * Time.deltaTime;

        float mouseY = mouseValue.y * rotateSpeed_Current * Time.deltaTime;
        yRotation = UpDown.transform.localRotation.eulerAngles.x - mouseY;


        //yRotation = Mathf.Clamp(yRotation, -maxRotationDown, maxRotationDown) ;


        if (yRotation > maxRotationDown && !(yRotation > 360 - maxRotationDown))
        {
            if (yRotation <= 180)
            {
                yRotation = maxRotationDown;
            }
            else
            {
                yRotation = 360 - maxRotationDown;
            }
        }


        UpDown.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);

        CharacterBody.Rotate(Vector3.up * mouseX);
    }

    public void ModifySpeed(float mult)
    {
        rotateSpeed_Current = rotateSpeed_Current * mult;
    }
    public void ResetSpeed()
    {
        rotateSpeed_Current = rotateSpeed;
    }

    public void AimSight(bool b, float mult)
    {
        isADS = b;
        if (b)
        {
            ModifySpeed(1 / mult);
            currentMult = mult;
        }
        else
        {
            ResetSpeed();
            //currentMult = 1;
        }
    }

    void AdjustAim()
    {
        if (isADS)
        {
            if (timeNow_Aim < 1)
            {
                timeNow_Aim += 5*Time.deltaTime;
                camera.fieldOfView = FOV - aimCurve.Evaluate(timeNow_Aim) * (FOV * (1-1/currentMult));
            }
            

        }
        else
        {
            if (timeNow_Aim >= 0)
            {
                timeNow_Aim -= 5 * Time.deltaTime;
                camera.fieldOfView = FOV - (aimCurve.Evaluate(timeNow_Aim) * (FOV * (1 - 1 / currentMult)));

            }
            else
            {
                camera.fieldOfView = FOV;
            }
        }
    }

    public float YRotation_adjusted()
    {
        return (YRotation + 90) % 180;
    }
}
