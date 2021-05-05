using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    public float rotateSpeed = 15f; //MouseSense
    [SerializeField] float rotateSpeed_Current;

    public Transform CharacterBody;
    public Transform UpDown;
    public Vector2 mouseValue;
    public Vector2 mouseValue_Nor;
    public float mouseValue_Mag;


    public float maxRotationDown = 40f;

    [SerializeField] float yRotation = 0f;
    [SerializeField] bool lookLock = false;


    [Header("ADS transition")]
    [SerializeField] float FOV = 0f;
    [SerializeField] AnimationCurve aimCurve;
    [SerializeField] private float timeNow_Aim = 0f;
    private bool isADS = false;
    private float currentMult = 1f;

    private Camera camera;

    public float YRotation { get => yRotation; set => yRotation = value; }
    public bool LookLock { get => lookLock; set => lookLock = value; }

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
        if (!lookLock)
        {
            MoveLook();
        }
        AdjustAim();
    }

    public void LookMouse(InputAction.CallbackContext callbackContext)
    {
        mouseValue = callbackContext.ReadValue<Vector2>() * (1 / Time.deltaTime) * 0.01f;
        mouseValue_Nor = mouseValue.normalized;
        mouseValue_Mag = mouseValue.magnitude;
    }
    public void LookController(InputAction.CallbackContext callbackContext)
    {
        mouseValue = callbackContext.ReadValue<Vector2>() * (1 / Time.deltaTime);
        mouseValue_Nor = mouseValue.normalized;
        mouseValue_Mag = mouseValue.magnitude;
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

        try
        {
            if (Time.timeScale > 0)
            {
                if (Mathf.Abs(YRotation) >= 0.0001f)
                {

                    Quaternion newUpDown = Quaternion.Euler(yRotation, 0, UpDown.transform.localRotation.eulerAngles.z);
                    
                    UpDown.transform.localRotation = newUpDown;
                }
                if (Mathf.Abs(mouseX) >= 0.0001f)
                {

                    CharacterBody.Rotate(Vector3.up * mouseX);
                }
            }
        }
        catch (System.Exception e)
        {

        }
    }

    public void ModifySpeed(float mult)
    {
        rotateSpeed_Current = rotateSpeed * mult;
    }
    public void ResetSpeed()
    {
        rotateSpeed_Current = rotateSpeed;
    }

    public void AimSight(bool b, float mult)
    {
        if (isADS && b)
        {
            timeNow_Aim = 0.5f;
        }

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
                timeNow_Aim += 5 * Time.deltaTime;
                camera.fieldOfView = FOV - aimCurve.Evaluate(timeNow_Aim) * (FOV * (1 - 1 / currentMult));
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

    /// <summary>
    /// to change rotation speed
    /// </summary>
    /// <param name="amount"> new rotation amount</param>
    public void SetRotationSpeed(float amount)
    {
        rotateSpeed = amount;
        rotateSpeed_Current = rotateSpeed;
    }
}
