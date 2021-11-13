﻿
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Mouse Sens")]
    [SerializeField] public float sensitivityX;
    [SerializeField] public float sensitivityY;

    [SerializeField] public float minY = -60f;
    [SerializeField] public float maxY = 60f;

    float lookX;
    float lookY;

    [Space]
    [Header("Jump stuff")]

    [SerializeField] public float gravity = -9.81f;
    [SerializeField] float jumpStrength = 8f;
    [SerializeField] float jumpVelocity;
    [SerializeField] LayerMask jumpLayerMask;
    [SerializeField] float doubleJumpStrength;
    [SerializeField] float coyoteJumpTime;
    private float groundCheckRadius = 0.3f;
    bool canDoubleJumped;
    bool coyoteJump;
    private bool isGrounded;


    [Header("Movement")]
    [SerializeField] float moveSpeed_Default;
    float moveSpeed;
    Vector3 moveDirection;
    float lastGroundedTime;
    Transform cam;
    Transform player;

    [Space]

    [Header("Dash")]
    [SerializeField] float dashDuration = 0.4f;
    [SerializeField] float dashSpeed = 5f;

    float dashStart = 0f;
    [SerializeField] float dashCooldown;
    [SerializeField] int dashCharges_Max;
    int dashCharges;

    [Space]
    [Header("Control Lock")]
    [SerializeField] bool disableControl = false;


    [Space]
    [Header("Other Components")]
    [SerializeField] Look lookScript;
    [SerializeField] PlayerGunDamageScript gunDamageScript;
    [SerializeField] PlayerInventorySystemScript playerInventorySystemScript;
    [SerializeField] PlayerInterationScript playerInterationScript;
    [SerializeField] PlayerVolumeControllerScript playerVolumeControllerScript;
    [SerializeField] AnsonTempUIScript ansonTempUIScript;
    [SerializeField] PlayerSoundScript playerSoundScript;
    CharacterController characterController;
    [SerializeField] private Animator animator;

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
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        player = transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform;
        canDoubleJumped = false;
        moveSpeed = moveSpeed_Default;
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {


        if (!disableControl)
        {

            Move();
            if (lookScript == null)
            {
                Look();
                //CameraTilt();

            }
            UpdateJumpAndGravity();

        }

        if (dashCharges < dashCharges_Max && Time.time > dashStart + dashCooldown)
        {
            dashCharges++;
            dashStart = Time.time;
            ansonTempUIScript.UpdateDashDisplay(dashCharges);
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

        characterController.Move(Quaternion.AngleAxis(transform.eulerAngles.y, transform.up) * moveDirection * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", moveDirection.magnitude);
    }

    public void Teleport(Vector3 pos)
    {
        characterController.enabled = false;
        transform.position = pos;
        characterController.enabled = true;

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
        moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
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
        if (context.performed)
        {
            if (isGrounded || (coyoteJump && Time.time - lastGroundedTime < coyoteJumpTime))
            {
                coyoteJump = false;
                canDoubleJumped = true;
                //jumped = new Vector3(0f, jumpStrength, 0f);
                jumpVelocity = jumpStrength;
                playerSoundScript.Play_Jump();

            }
            else
            {
                if (canDoubleJumped)
                {
                    coyoteJump = false;
                    //jumped = new Vector3(0f, doubleJumpSpeed, 0f);
                    jumpVelocity = Mathf.Max(doubleJumpStrength + jumpVelocity, doubleJumpStrength);
                    canDoubleJumped = false;
                    playerSoundScript.Play_Jump();
                    animator.SetTrigger("Jump");

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
            gunDamageScript.PressFire(true);
        }
        else if (callbackContext.canceled)
        {
            gunDamageScript.PressFire(false);
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
            gunDamageScript.PressADS(true);
        }
        else if (callbackContext.canceled)
        {
            gunDamageScript.PressADS(false);

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



    public void UpdateJumpAndGravity()
    {
        GroundCheck();
        HeadCheck();
        characterController.Move(new Vector3(0, jumpVelocity * Time.deltaTime, 0));
        animator.SetBool("Grounded", isGrounded);
        if (!isGrounded)
        {
            jumpVelocity += gravity * Time.deltaTime;
        }
        else
        {
            if (jumpVelocity < groundCheckRadius * gravity / 2f)
            {
                jumpVelocity = groundCheckRadius * gravity / 2f;
            }
            coyoteJump = true;
            canDoubleJumped = true;
            lastGroundedTime = Time.time;
        }
    }

    public void GroundCheck()
    {
        Vector3 offsetHeight = new Vector3(0, characterController.center.y - (characterController.height / 2f), 0);
        isGrounded = Physics.CheckSphere(characterController.transform.position + offsetHeight, groundCheckRadius, jumpLayerMask, QueryTriggerInteraction.Ignore);
    }
    public void HeadCheck()
    {
        Vector3 offsetHeight = new Vector3(0, characterController.center.y + (characterController.height / 2f), 0);
        bool hitHead = Physics.CheckSphere(characterController.transform.position + offsetHeight, groundCheckRadius, jumpLayerMask, QueryTriggerInteraction.Ignore);
        if (hitHead)
        {
            jumpVelocity = Mathf.Min(jumpVelocity, 0f);
        }
    }

}