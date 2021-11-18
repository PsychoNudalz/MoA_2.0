using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class Look : MonoBehaviour
{
    public float rotateSpeed = 15f; //MouseSense
    [SerializeField] float ADSMultiplier;
    [SerializeField] float rotateSpeed_Current;

    public Transform CharacterBody;
    public Vector2 mouseValue;
    public Vector2 mouseValue_Nor;
    public float mouseValue_Mag;


    public float maxRotationDown = 40f;
    public float maxRotationSide = 15f;

    [SerializeField] float yRotation = 0f;
    [SerializeField] bool lookLock = false;


    [Header("ADS transition")]
    [SerializeField] CinemachineVirtualCamera camera;
    [SerializeField] LensSettings cameraLens;
    [SerializeField] float FOV = 0f;
    [SerializeField] AnimationCurve aimCurve;
    [SerializeField] private float timeNow_Aim = 0f;
    private bool isADS = false;
    private float currentMult = 1f;


    [Header("Head Layers")]
    [SerializeField] Transform controllerLayer;
    [SerializeField] Transform recoilLayer;
    [SerializeField] Transform weaponLayer;

    [Header("Camera Recoil")]
    [SerializeField] Vector2 targetRecoil = new Vector2();
    [SerializeField] float timeToRecenterRecoil;
    [SerializeField] float timeToRecenterRecoil_Little;
    [SerializeField] bool isRecenter = true;
    [SerializeField] float controllerXOriginal;
    [SerializeField] float controllerXRemaing;


    public float YRotation { get => yRotation; set => yRotation = value; }
    public bool LookLock { get => lookLock; set => lookLock = value; }

    //[SerializeField] bool disableControl = false;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rotateSpeed_Current = rotateSpeed;
        //camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraLens = camera.m_Lens;
        FOV = cameraLens.FieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if (!lookLock)
        {
            MoveLook();
            UpdateRecoil();
            AdjustAim();
        }
    }

    private void OnApplicationQuit()
    {
        cameraLens.FieldOfView = FOV;
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
        //yRotation = controllerLayer.transform.localRotation.eulerAngles.x - mouseY;
        yRotation = yRotation - mouseY;
        float recoilX = -XRotation_adjust(recoilLayer.localEulerAngles.x);
        yRotation = Mathf.Clamp(yRotation, -maxRotationDown + recoilX, maxRotationDown + recoilX);
        //print($"y clamp {-maxRotationDown + recoilX} {maxRotationDown + recoilX}");




        //yRotation = Mathf.Clamp(yRotation, -maxRotationDown, maxRotationDown) ;

        //if (yRotation > maxRotationDown - recoilLayer.localEulerAngles.z && !(yRotation > 360 - maxRotationDown-recoilLayer.localEulerAngles.z))
        //{
        //    if (yRotation <= 180)
        //    {
        //        yRotation = maxRotationDown - recoilLayer.localEulerAngles.z;
        //    }
        //    else
        //    {
        //        yRotation = 360 - maxRotationDown - recoilLayer.localEulerAngles.z;
        //    }
        //}

        try
        {
            if (Time.timeScale > 0)
            {
                if (Mathf.Abs(YRotation) >= 0.0001f)
                {

                    Quaternion newUpDown = Quaternion.Euler(yRotation, 0, 0);
                    Vector3 temp = controllerLayer.localEulerAngles;
                    temp.x = yRotation;
                    controllerLayer.localRotation = newUpDown;
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
            ModifySpeed((1 / mult) * ADSMultiplier);
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
                cameraLens.FieldOfView = FOV - aimCurve.Evaluate(timeNow_Aim) * (FOV * (1 - 1 / currentMult));
            }


        }
        else
        {
            if (timeNow_Aim >= 0)
            {
                timeNow_Aim -= 5 * Time.deltaTime;
                cameraLens.FieldOfView = FOV - (aimCurve.Evaluate(timeNow_Aim) * (FOV * (1 - 1 / currentMult)));

            }
            else
            {
                cameraLens.FieldOfView = FOV;
            }
        }
        camera.m_Lens = cameraLens;
    }

    public float YRotation_adjusted()
    {
        return (YRotation + 90) % 180;
    }

    public float XRotation_adjust(float x)
    {
        if (x > 180)
        {
            x = x - 360;
        }
        return x;
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

    public void SetADSMultiplier(float amount)
    {
        ADSMultiplier = amount;
    }

    void UpdateRecoil()
    {
        //Move up
        Quaternion targetRotation = Quaternion.Euler(-targetRecoil.x, targetRecoil.y, 0) * recoilLayer.localRotation;
        Vector3 targetEuler = targetRotation.eulerAngles;
        if (targetEuler.x > 180 && targetEuler.x < 360 - maxRotationDown)
        {
            print("over recoil");
            targetEuler.x = -maxRotationDown;
            targetEuler.y = 0;
            targetEuler.z = 0;
            targetRotation = Quaternion.Euler(targetEuler);
            targetRecoil = new Vector2(Mathf.Min(targetRecoil.x, 20f), 0);
            recoilLayer.localRotation = Quaternion.Lerp(recoilLayer.localRotation, targetRotation, Time.deltaTime * 10f);
            float recoilX = -XRotation_adjust(recoilLayer.localEulerAngles.x);
            //yRotation = Mathf.Clamp(yRotation, -maxRotationDown + recoilX, maxRotationDown + recoilX);

        }
        else
        {

            recoilLayer.localRotation = Quaternion.Lerp(recoilLayer.localRotation, targetRotation, Time.deltaTime * 10f);

        }
        //recoilLayer.localEulerAngles = new Vector3(recoilLayer.localEulerAngles.x, recoilLayer.localEulerAngles.y, 0);
        //Move Down
        if (isRecenter)
        {
            ReadjustRecoil();
        }
        else
        {
            ReadjustRecoil_Little();
        }
    }

    public void AddRecoil(Vector2 recoil)
    {

        Quaternion targetRotation = recoilLayer.localRotation;
        Vector3 targetEuler = targetRotation.eulerAngles;
        // print($"add recoil {targetRecoil}");
        if (targetEuler.x > 180 && targetEuler.x < 360 - maxRotationDown)
        {
            print("over recoil on add");

        } else if (Mathf.Abs(targetRecoil.y) > maxRotationSide)
        {
            print("over side recoil on add");

        }
        else
        {
            targetRecoil += recoil;

        }
    }


    public void SetIsRecenter(bool b)
    {
        isRecenter = b;
        if (!b)
        {
            controllerXOriginal = yRotation;

        }
        else
        {
            controllerXRemaing = -(controllerXOriginal - XRotation_adjust(controllerLayer.localEulerAngles.x));
            //print($"{controllerXRemaing} {controllerLayer.localEulerAngles.x} {recoilLayer.localEulerAngles.x}");
            RecoilControlControllerLayer();

        }
    }
    void ReadjustRecoil()
    {
        targetRecoil = Vector2.Lerp(targetRecoil, new Vector2(0, 0), Time.deltaTime * 2 / timeToRecenterRecoil);
        if (targetRecoil.magnitude < 0.001f)
        {
            targetRecoil = new Vector2(0, 0);
        }

        recoilLayer.localRotation = Quaternion.Lerp(recoilLayer.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 2 / timeToRecenterRecoil);
        recoilLayer.localEulerAngles = new Vector3(recoilLayer.localEulerAngles.x, recoilLayer.localEulerAngles.y, 0);
    }

    private void RecoilControlControllerLayer()
    {
        //player looks higher
        if (controllerXRemaing <= 0f)
        {
            //print("player looks higher");
        }
        //player recoil controls above the original point
        else if (controllerXOriginal > XRotation_adjust(controllerLayer.localEulerAngles.x) + XRotation_adjust(recoilLayer.localEulerAngles.x))
        {
            //controllerLayer.localRotation = Quaternion.Euler(-controllerXRemaing, 0f, 0f) * controllerLayer.localRotation;
            yRotation = yRotation - controllerXRemaing;

            recoilLayer.localRotation = Quaternion.Euler(controllerXRemaing, 0f, 0f) * recoilLayer.localRotation;
            //controllerXRemaing = 0;
            //print("player recoil controls above the original point");

        }
        //player recoil controls below
        else
        {
            //print("player recoil controls below");
            targetRecoil = targetRecoil.normalized;
            controllerXRemaing = XRotation_adjust(recoilLayer.localEulerAngles.x);
            //print(controllerXRemaing);
            //controllerLayer.localRotation = Quaternion.Euler(controllerXRemaing, 0f, 0f) * controllerLayer.localRotation;
            yRotation = yRotation + controllerXRemaing;

            recoilLayer.localRotation = Quaternion.Euler(-controllerXRemaing, 0f, 0f) * recoilLayer.localRotation;
            //controllerXRemaing = 0;
        }

    }

    void ReadjustRecoil_Little()
    {
        targetRecoil = Vector2.Lerp(targetRecoil, targetRecoil, Time.deltaTime * 2 / timeToRecenterRecoil_Little);
        recoilLayer.localRotation = Quaternion.Lerp(recoilLayer.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * 2 / timeToRecenterRecoil_Little);

        recoilLayer.localEulerAngles = new Vector3(recoilLayer.localEulerAngles.x, recoilLayer.localEulerAngles.y, 0);

    }

}
