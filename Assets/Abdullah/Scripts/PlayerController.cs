﻿
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float gravity = -9.81f;
    public float jumpSpeed = 8f;


    [SerializeField] public float sensitivityX;
    [SerializeField] public float sensitivityY;

    [SerializeField] public float minY = -60f;
    [SerializeField] public float maxY = 60f;

    [SerializeField] public float tilt = 20;


    [Header("Jump stuff")]
    Vector3 jumped;
    [SerializeField] float jumpHeadDetection = 0.2f;
    [SerializeField] LayerMask jumpLayerMask;

    [Space]
    float moveSpeed;
    [SerializeField] float moveSpeed_Default;

    CharacterController controller;
    Vector3 moveDirection;
    bool jump;
    bool run;
    [SerializeField] bool coyoteJump;
    [SerializeField] float coyoteJumpTime;
    float lastGroundedTime;

    Transform cam;
    Transform player;

    float lookX;
    float lookY;

    [SerializeField] Camera cam1;
    bool moving;
    Vector2 isMoving;
    bool tilted;

    Vector3 dashRange;
    float dashDistance = 10f;
    [SerializeField] float dashDuration = 0.4f;
    [SerializeField] float dashSpeed = 5f;

    [SerializeField] Look lookScript;

    bool canDoubleJumped;
    [SerializeField] float doubleJumpSpeed;

    float dashStart = 0f;
    [SerializeField] float dashCooldown;
    [SerializeField] int dashCharges_Max;
    int dashCharges;
    [Space]
    [SerializeField] bool disableControl = false;


    [Space]
    [Header("Other Components")]
    [SerializeField] PlayerGunDamageScript gunDamageScript;
    [SerializeField] PlayerInventorySystemScript playerInventorySystemScript;
    [SerializeField] PlayerInterationScript playerInterationScript;
    [SerializeField] PlayerVolumeControllerScript playerVolumeControllerScript;
    [SerializeField] AnsonTempUIScript ansonTempUIScript;
    [SerializeField] PlayerSoundScript playerSoundScript;

    public PlayerGunDamageScript GunDamageScript { get => gunDamageScript; set => gunDamageScript = value; }
    public PlayerInventorySystemScript PlayerInventorySystemScript { get => playerInventorySystemScript; set => playerInventorySystemScript = value; }
    public PlayerInterationScript PlayerInterationScript { get => playerInterationScript; set => playerInterationScript = value; }
    public AnsonTempUIScript AnsonTempUIScript { get => ansonTempUIScript; set => ansonTempUIScript = value; }
    public bool DisableControl { get => disableControl; set => disableControl = value; }
    public int DashCharges { get => dashCharges; }
    public PlayerVolumeControllerScript PlayerVolumeControllerScript { set => playerVolumeControllerScript = value; }
    public PlayerSoundScript PlayerSoundScript { get => playerSoundScript; set => playerSoundScript = value; }



    // Start is called before the first frame update
    void Start()
    {
        tilted = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        player = transform;
        cam = cam1.transform;
        canDoubleJumped = false;
        moveSpeed = moveSpeed_Default;
    }

    // Update is called once per frame
    void Update()
    {

        //moveDirection = transform.TransformDirection(moveDirection);

        if (!disableControl)
        {

            Move();
            if (lookScript == null)
            {
                Look();
                //CameraTilt();

            }
            if (!controller.isGrounded)
            {

                //print("Adding gravity");
                //controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
                jumped.y -= gravity * Time.deltaTime;
                //Debug.DrawRay(controller.center + new Vector3(0, controller.height / 2f, 0))
                if (Physics.Raycast(transform.position+ controller.center+new Vector3(0, controller.height / 2f, 0), transform.up, jumpHeadDetection, jumpLayerMask))
                {
                    print("Playuer hit head");
                    jumped.y = Mathf.Clamp(.1f, 0, jumped.y);
                }
            }
            else
            {
                coyoteJump = true;
                canDoubleJumped = true;
                lastGroundedTime = Time.time;
                if (jumped.y != 0)
                {
                    jumped.y = -4f;
                }
                
            }
        }

        if (dashCharges < dashCharges_Max && Time.time > dashStart + dashCooldown)
        {
            dashCharges++;
            dashStart = Time.time;
            ansonTempUIScript.UpdateDashDisplay(dashCharges);
        }
    }




    void CameraTilt()
    {
        if (isMoving.x == 0)
        {
            Vector3 eulerRotation = transform.rotation.eulerAngles;
            cam1.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
            tilted = false;
        }
    }

    void Look()
    {
        lookY = Mathf.Clamp(lookY, minY, maxY);
        player.localEulerAngles = new Vector3(0, lookX, 0);
        cam.localEulerAngles = new Vector3(lookY, 0, 0);
    }

    void Move()
    {

        controller.Move(Quaternion.AngleAxis(transform.eulerAngles.y, transform.up) * moveDirection * moveSpeed * Time.deltaTime);
        controller.Move(jumped * Time.deltaTime);
    }

    public void Teleport(Vector3 pos)
    {
        controller.enabled = false;
        transform.position = pos;
        controller.enabled = true;

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="b">false to lock controller</param>
    public void SetControlLock(bool b)
    {
        disableControl = !b;
        lookScript.LookLock = !b;
        if (b)
        {
            moveDirection = new Vector3();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (disableControl)
        {
            return;
        }
        isMoving = context.ReadValue<Vector2>();
        moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        //moveDirection = transform.TransformDirection(moveDirection);
        if (isMoving.x != 0)
        {
            if (isMoving.x < 0 && tilted == false)
            {
                cam1.transform.Rotate(new Vector3(0, 0, tilt) * Time.deltaTime);
                tilted = true;

            }
            else if (isMoving.x > 0 && tilted == false)
            {
                cam1.transform.Rotate(new Vector3(0, 0, -tilt) * Time.deltaTime);
                tilted = true;
            }
        }


    }

    public void OnLook_Mouse(InputAction.CallbackContext context)
    {
        if (disableControl)
        {
            return;
        }
        //lookX += context.ReadValue<Vector2>().x * sensitivityX * Time.deltaTime;
        //lookY -= context.ReadValue<Vector2>().y * sensitivityY * Time.deltaTime;
        lookScript.LookMouse(context);
    }
    public void OnLook_Controller(InputAction.CallbackContext context)
    {
        if (disableControl)
        {
            return;
        }
        //lookX += context.ReadValue<Vector2>().x * sensitivityX * Time.deltaTime;
        //lookY -= context.ReadValue<Vector2>().y * sensitivityY * Time.deltaTime;
        lookScript.LookController(context);
    }

    public void OnJump(InputAction.CallbackContext context)
    {


        if (disableControl)
        {
            return;
        }
        jump = context.performed;
        if (context.performed)
        {
            if (controller.isGrounded || (coyoteJump && Time.time - lastGroundedTime < coyoteJumpTime))
            {
                coyoteJump = false;
                canDoubleJumped = true;
                jumped = new Vector3(0f, jumpSpeed, 0f);
                playerSoundScript.Play_Jump();

            }
            else
            {
                if (canDoubleJumped)
                {
                    coyoteJump = false;
                    jumped = new Vector3(0f, doubleJumpSpeed, 0f);
                    canDoubleJumped = false;
                    playerSoundScript.Play_Jump();

                }
            }

        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (disableControl)
        {
            return;
        }

        if (context.performed)
        {
            if (dashCharges > 0 && moveDirection.magnitude > 0)
            {
                /*
                dashRange = transform.TransformDirection(moveDirection) * (dashDistance * 100);
                controller.Move(dashRange * Time.deltaTime);
                */
                playerSoundScript.Play_Dash();
                dashCharges--;
                dashStart = Time.time;
                playerVolumeControllerScript.PlayLD();
                StartCoroutine(DashCoroutine());
                ansonTempUIScript.UpdateDashDisplay(dashCharges);
            }
        }

    }

    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }
        if (callbackContext.performed)
        {
            gunDamageScript.Fire(true);
        }
        else if (callbackContext.canceled)
        {
            gunDamageScript.Fire(false);
        }
    }


    public void Aim(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }
        if (callbackContext.performed)
        {
            gunDamageScript.ADS_On();
        }
        else if (callbackContext.canceled)
        {
            gunDamageScript.ADS_Off();

        }
    }
    public void SwapToWeapon1(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }
        if (callbackContext.performed)
        {
            playerInventorySystemScript.SwapToWeapon(0);
        }
    }
    public void SwapToWeapon2(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }
        if (callbackContext.performed)
        {
            playerInventorySystemScript.SwapToWeapon(1);
        }
    }
    public void SwapToWeapon3(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }
        if (callbackContext.performed)
        {
            playerInventorySystemScript.SwapToWeapon(2);
        }
    }

    public void CycleWeapon_Next(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }
        if (callbackContext.performed)
        {
            playerInventorySystemScript.CycleWeapon(true);
        }
    }

    public void CycleWeapon_Prev(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }
        if (callbackContext.performed)
        {
            playerInventorySystemScript.CycleWeapon(false);
        }
    }
    public void Reload()
    {
        gunDamageScript.Reload();
    }

    public void Interact(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            InteractableScript interactable = playerInterationScript.CurrentFocus;
            if (interactable == null)
            {
                return;
            }
            if (interactable is WeaponPickUpInteractableScript)
            {
                playerInventorySystemScript.PickUpNewGun(((WeaponPickUpInteractableScript)interactable).ConnectedGun);
                playerInterationScript.ClearInteractable();
            }
            else
            {
                playerInterationScript.useInteractable();
            }
        }
    }

    public void OnPauseGame(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            ansonTempUIScript.CloseAllMenus();
            if (FindObjectOfType<PauseMenu>().TogglePauseMenu())
            {
                SetControlLock(false);
            }
            else
            {
                SetControlLock(true);
            }
        }
    }

    IEnumerator DashCoroutine()
    {
        moveSpeed = moveSpeed * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        moveSpeed = moveSpeed_Default;
    }
    private void OnEnable()
    {
        moveSpeed = moveSpeed_Default;

    }
    /// <summary>
    /// to change mouse sensitivity
    /// </summary>
    /// <param name="amount"> new sensitivity amount</param>
    public void SetSensitivity(float amount)
    {
        sensitivityX = amount;
        sensitivityY = amount;
        if (lookScript != null)
        {
            lookScript.SetRotationSpeed(amount);
        }
        print("Player Update sensitivity to:" + amount);
    }

    public void SetADSSensitivity(float amount)
    {
        if (lookScript != null)
        {
            lookScript.SetADSMultiplier(amount);
        }
    }


}