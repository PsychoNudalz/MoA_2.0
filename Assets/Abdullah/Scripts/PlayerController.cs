using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [Header("Mouse Sens")]
    [SerializeField]
    public float sensitivityX;

    [SerializeField]
    public float sensitivityY;

    [SerializeField]
    public float minY = -60f;

    [SerializeField]
    public float maxY = 60f;

    float lookX;
    float lookY;

    [Space]
    [Header("Jump stuff")]
    [SerializeField]
    public float gravity = -9.81f;

    [SerializeField]
    float jumpStrength = 8f;

    [SerializeField]
    float jumpVelocity;

    [SerializeField]
    LayerMask jumpLayerMask;

    [SerializeField]
    float doubleJumpStrength;

    [SerializeField]
    float coyoteJumpTime;

    private float groundCheckRadius = 0.3f;
    bool canDoubleJumped;
    bool coyoteJump;
    private bool isGrounded;


    [Header("Movement")]
    [SerializeField]
    float moveSpeed_Default;

    [SerializeField]
    private float moveAcceleration = .3f;

    //float moveSpeed_Cap;
    private float moveSpeed_Current = 0f;
    private float moveSpeed_Target = 0f;

    Vector3 moveDirection;
    float lastGroundedTime;
    Transform cam;
    Transform player;

    [Space]
    [Header("Dash")]
    [SerializeField]
    float dashDuration = 0.4f;

    [FormerlySerializedAs("dashSpeed")]
    [SerializeField]
    float dashMultiplier = 5f;

    float dashStart = 0f;

    [SerializeField]
    float dashCooldown;

    [SerializeField]
    int dashCharges_Max;

    int dashCharges;

    [Space]
    private float height_Original;

    private float radius_Original;

    private float height_Target;

    [Header("Crouch")]
    [SerializeField]
    private float heightChangeSpeed = 1f;

    [SerializeField]
    private float crouchHeightMultiplier = 0.5f;

    [SerializeField]
    private float crouchSpeedMultiplier = .65f;

    [Header("Slide")]
    [SerializeField]
    private float slideHeightMultiplier = 0.5f;

    [SerializeField]
    private float slideSpeedMultiplier = 2f;

    [SerializeField]
    private float slideDuration = 5f;

    [SerializeField]
    private AnimationCurve slideCurve;


    private bool isCrouch = false;
    private bool isSlide = false;
    private float slideStartTime = 0;


    [Space(10)]
    [Header("Control Lock")]
    [SerializeField]
    bool disableControl = false;


    [Space]
    [Header("Other Components")]
    [SerializeField]
    Look lookScript;

    [SerializeField]
    PlayerGunDamageScript gunDamageScript;

    [SerializeField]
    PlayerInventorySystemScript playerInventorySystemScript;

    [SerializeField]
    PlayerInterationScript playerInterationScript;

    [SerializeField]
    PlayerVolumeControllerScript playerVolumeControllerScript;

    [SerializeField]
    AnsonTempUIScript ansonTempUIScript;

    [SerializeField]
    PlayerSoundScript playerSoundScript;

    CharacterController characterController;

    [SerializeField]
    private Animator animator;

    public PlayerGunDamageScript GunDamageScript
    {
        get => gunDamageScript;
        set => gunDamageScript = value;
    }

    public PlayerInventorySystemScript PlayerInventorySystemScript
    {
        get => playerInventorySystemScript;
        set => playerInventorySystemScript = value;
    }

    public PlayerInterationScript PlayerInterationScript
    {
        get => playerInterationScript;
        set => playerInterationScript = value;
    }

    public AnsonTempUIScript AnsonTempUIScript
    {
        get => ansonTempUIScript;
        set => ansonTempUIScript = value;
    }

    public bool DisableControl
    {
        get => disableControl;
        set => disableControl = value;
    }

    public int DashCharges
    {
        get => dashCharges;
    }

    public PlayerVolumeControllerScript PlayerVolumeControllerScript
    {
        set => playerVolumeControllerScript = value;
    }

    public PlayerSoundScript PlayerSoundScript
    {
        get => playerSoundScript;
        set => playerSoundScript = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        player = transform;
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform;
        canDoubleJumped = false;
        //moveSpeed_Cap = moveSpeed_Default;
        if (!animator)
        {
            animator = GetComponent<Animator>();
        }

        height_Original = characterController.height;
        height_Target = height_Original;
        radius_Original = characterController.radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (!disableControl)
        {
            UpdateVelocity();
            Move();
            if (lookScript == null)
            {
                Look();
                //CameraTilt();
            }

            UpdateJumpAndGravity();
        }


        UpdatePlayerHeight();


        //Dash Recharge
        if (dashCharges < dashCharges_Max && Time.time > dashStart + dashCooldown)
        {
            dashCharges++;
            dashStart = Time.time;
            ansonTempUIScript.UpdateDashDisplay(dashCharges);
        }
    }

    private void LateUpdate()
    {
        UpdatePlayerHeight();

        //check for stopping slide
    }


    void Look()
    {
        lookY = Mathf.Clamp(lookY, minY, maxY);
        player.localEulerAngles = new Vector3(0, lookX, 0);
        cam.localEulerAngles = new Vector3(lookY, 0, 0);
    }

    void Move()
    {
        animator.SetFloat("Speed", moveDirection.magnitude);
        if (isSlide)
        {
            characterController.Move(transform.forward * moveSpeed_Current * Time.deltaTime);
        }
        else
        {
            characterController.Move(Quaternion.AngleAxis(transform.eulerAngles.y, transform.up) * moveDirection *
                                     moveSpeed_Current * Time.deltaTime);
        }
    }

    void UpdateVelocity()
    {
        if (isSlide)
        {
            if (moveSpeed_Current <= slideCurve.Evaluate(.999f))
            {
                OnSlide_End();
                Crouch();
            }

            //moveSpeed_Current = Mathf.Max(moveSpeed_Current - slideDecel * Time.deltaTime, 0f);

            moveSpeed_Current =
                slideCurve.Evaluate((Time.time - slideStartTime) / slideDuration) * slideSpeedMultiplier *
                moveSpeed_Default;
        }

        else
        {
            if (moveSpeed_Current > moveSpeed_Target)
            {
                moveSpeed_Current = Mathf.Max(moveSpeed_Current - moveAcceleration * Time.deltaTime, moveSpeed_Target);
            }
            else if (moveSpeed_Current < moveSpeed_Target)
            {
                moveSpeed_Current = Mathf.Min(moveSpeed_Current + moveAcceleration * Time.deltaTime, moveSpeed_Target);
            }

            // if (moveDirection.magnitude <= 0.1f)
            // {
            //     moveVelocity_Current = Mathf.Max(moveVelocity_Current - moveAcceleration * Time.deltaTime, 0f);
            // }else
            // {
            //     if (isCrouch)
            //     {
            //         moveVelocity_Current = Mathf.Min(moveSpeed_Cap * crouchSpeedMultiplier,
            //             moveVelocity_Current + moveAcceleration * Time.deltaTime);
            //         
            //     }
            //     else
            //     {
            //         moveVelocity_Current = Mathf.Min(moveSpeed_Cap, moveVelocity_Current + moveAcceleration * Time.deltaTime);
            //     }
            // }
        }
    }

    public void SetMoveSpeed_Target(float f)
    {
        moveSpeed_Target = f;
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

        float newMoveSpeed = 0f;
        if (moveDirection.magnitude > 0.1f)
        {
            if (isCrouch)
            {
                newMoveSpeed = crouchSpeedMultiplier * moveSpeed_Default;
            }
            else if (isSlide)
            {
            }
            else
            {
                newMoveSpeed = moveSpeed_Default;
            }
        }
        else
        {
            if (!isSlide)
            {
                newMoveSpeed = 0f;
            }
        }

        if (Math.Abs(newMoveSpeed - moveSpeed_Target) > 0.001f)
        {
            SetMoveSpeed_Target(newMoveSpeed);
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

        if (context.performed)
        {
            if (isCrouch)
            {
                UnCrouch();
            }

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
            if (isCrouch)
            {
                UnCrouch();
            }

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

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (!isCrouch && isGrounded)
            {
                Crouch();
            }
            else
            {
                UnCrouch();
            }
        }
        else if (context.canceled)
        {
            if (isSlide)
            {
                OnSlide_End();
            }
        }
    }

    private void UnCrouch()
    {
        // print("UnCrouch");
        height_Target = height_Original;
        isCrouch = false;
        SetMoveSpeed_Target(moveSpeed_Default);
    }

    private void Crouch()
    {
        // print("Crouch");
        height_Target = height_Original * crouchHeightMultiplier;
        isCrouch = true;
        SetMoveSpeed_Target(moveSpeed_Default * crouchSpeedMultiplier);
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (isGrounded&& context.performed && moveDirection.z > 0.1f && moveSpeed_Current > moveSpeed_Default * .5f)
        {
            // print("Slide pressed");
            OnSlide();
        }
    }

    public void OnSlide()
    {
        height_Target = height_Original * slideHeightMultiplier;
        // moveSpeed_Current = moveSpeed_Default * slideSpeedMultiplier;
        SetMoveSpeed_Target(moveSpeed_Default * slideSpeedMultiplier);
        //moveSpeed_Current = moveSpeed_Default * slideSpeedMultiplier;
        isSlide = true;
        slideStartTime = Time.time;
    }

    public void OnSlide_End()
    {
        UnCrouch();
        isSlide = false;
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
                playerInventorySystemScript.PickUpNewGun(((WeaponPickUpInteractableScript) interactable).ConnectedGun);
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
        moveSpeed_Target = moveSpeed_Default * dashMultiplier;
        moveSpeed_Current = moveSpeed_Target;
        yield return new WaitForSeconds(dashDuration);
        moveSpeed_Target = moveSpeed_Default;
        moveSpeed_Current = moveSpeed_Target;
    }

    private void OnEnable()
    {
        // moveSpeed_Cap = moveSpeed_Default;
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


    void UpdateJumpAndGravity()
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

    void GroundCheck()
    {
        Vector3 offsetHeight = new Vector3(0, characterController.center.y - (characterController.height / 2f), 0);
        isGrounded = Physics.CheckSphere(characterController.transform.position + offsetHeight, groundCheckRadius,
            jumpLayerMask, QueryTriggerInteraction.Ignore);
    }

    void HeadCheck()
    {
        Vector3 offsetHeight = new Vector3(0, characterController.center.y + (characterController.height / 2f), 0);
        bool hitHead = Physics.CheckSphere(characterController.transform.position + offsetHeight, groundCheckRadius,
            jumpLayerMask, QueryTriggerInteraction.Ignore);
        if (hitHead)
        {
            jumpVelocity = Mathf.Min(jumpVelocity, 0f);
        }
    }

    void UpdatePlayerHeight()
    {
        var height = characterController.height;
        if (Math.Abs(height - height_Target) > .01f)
        {
            // characterController.height =
            //     Mathf.Lerp(characterController.height, height_Target, heightChangeSpeed * Time.deltaTime);
            //
            if (height_Target > characterController.height)
            {
                height += Time.deltaTime * heightChangeSpeed;
                height = Mathf.Min(height, height_Target);
            }
            else if (height_Target < characterController.height)
            {
                height -= Time.deltaTime * heightChangeSpeed;
                height = Mathf.Max(height, height_Target);
            }

            characterController.height = height;
        }

        if (height / 2f < radius_Original)
        {
            characterController.radius = height / 2f;
        }
        else if (Math.Abs(characterController.radius - radius_Original) > 0.001f)
        {
            characterController.radius = radius_Original;
        }
    }
}