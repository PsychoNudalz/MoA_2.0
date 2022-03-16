using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;


public class PlayerController : MonoBehaviour
{
    // [Header("Movement Mode")]
    // [SerializeField]
    // PlayerMovementEnum playerMovementEnum = PlayerMovementEnum.Walk;
    //
    // enum PlayerMovementEnum
    // {
    //     Walk,
    //     Slide,
    //     Stick,
    //     OnMantle
    // }
    //
    //
    // [Space(10f)]
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

    [SerializeField]
    private float inAirControl = 3f;

    private float groundCheckRadius = 0.3f;
    bool canDoubleJumped;
    bool coyoteJump;
    private bool isGrounded;


    [Header("Movement")]
    [SerializeField]
    float moveSpeed_Default;

    [SerializeField]
    private float moveSpeed_Multiplier = 1;

    [SerializeField]
    private float moveAcceleration = 24f;

    //float moveSpeed_Cap;
    private float moveSpeed_Current = 0f;
    private float moveSpeed_Target = 0f;

    private Vector3 inputDirection;
    Vector3 moveDirection;
    float lastGroundedTime;
    Transform cam;
    Transform player;

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

    private float height_Original;

    private float radius_Original;

    private float height_Target;

    [Header("Crouch")]
    [SerializeField]
    private float heightChangeSpeed = 5f;

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

    [SerializeField]
    private Transform slideCastPoint;

    [SerializeField]
    private HandPositionPointer slideHandPointer;


    private bool isCrouch = false;
    private bool isSlide = false;
    private float slideStartTime = 0;


    [Header("Mantling")]
    [SerializeField]
    private TriggerDetector headVaultDetector;

    [SerializeField]
    private TriggerDetector frontWallDetector;

    [SerializeField]
    private Transform mantlingCastPoint;

    [SerializeField]
    private float mantleTime;

    [SerializeField]
    private float slopLimit_Mantle = 35f;

    [SerializeField]
    private float stepLimit_Mantle = 1f;

    [SerializeField]
    private float mantleSpeed = 1f;

    [SerializeField]
    private float mantleEuler = 85f;

    [SerializeField]
    private Transform mantleHandCast;

    [SerializeField]
    private HandPositionPointer mantleHandPointer;


    private float slopLimit_Default = 35f;

    private float stepLimit_Default = 2f;
    private bool isMantle = false;
    private Vector3 mantleMoveDir = new Vector3();
    private Vector3 mantleHandPos = new Vector3();


    [Header("Wall Stick/ Bounce")]
    [SerializeField]
    float bounceSpeedMult = 3f;

    [SerializeField]
    float bounceYMult = 1.5f;

    [SerializeField]
    private TriggerDetector sideDetector;

    [SerializeField]
    private HandPositionPointer stickHandPointer;

    [SerializeField]
    private Transform stickHandTransform;

    private bool isStick = false;
    private Vector3 stickPoint;
    private RaycastHit handStickHit;

    [Header("Melee")]
    [SerializeField]
    private PlayerMelee playerMelee;

    [SerializeField]
    private HandPositionPointer meleeHandPointer;


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

    [FormerlySerializedAs("ansonTempUIScript")]
    [SerializeField]
    PlayerUIScript playerUIScript;

    [SerializeField]
    PlayerSoundScript playerSoundScript;


    public PlayerMelee PlayerMelee
    {
        get => playerMelee;
        set => playerMelee = value;
    }

    CharacterController characterController;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform playerHitBox;


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

    public PlayerUIScript PlayerUIScript
    {
        get => playerUIScript;
        set => playerUIScript = value;
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

    public float MoveSpeedCurrent => moveSpeed_Current;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = transform;
        canDoubleJumped = false;
        //moveSpeed_Cap = moveSpeed_Default;

        AssignComponents();

        height_Original = characterController.height;
        SetPlayerHeight(1);
        radius_Original = characterController.radius;
        slopLimit_Default = characterController.slopeLimit;
        stepLimit_Default = characterController.stepOffset;
    }

    [ContextMenu("Assign Components")]
    private void AssignComponents()
    {
        if (!cam)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().transform;
        }

        if (!characterController)
        {
            characterController = GetComponent<CharacterController>();
        }

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

        UpdateUIControlPrompts();

        if (isMantle)
        {
            UpdateMantleHand();
        }
        else if (isStick)
        {
            UpdateStickHandPosition();
        }
        else if (isSlide)
        {
            UpdateSlideHandPosition();
        }


        //Dash Recharge
        if (dashCharges < dashCharges_Max && Time.time > dashStart + dashCooldown)
        {
            dashCharges++;
            dashStart = Time.time;
            playerUIScript.UpdateDashDisplay(dashCharges);
        }
    }

    private void LateUpdate()
    {
        UpdatePlayerHeight();

        //check for stopping slide
    }

    private void OnDrawGizmosSelected()
    {
        AssignComponents();
        Vector3 offsetHeight = new Vector3(0, characterController.center.y - (characterController.height / 2f), 0);
        Gizmos.DrawSphere(characterController.transform.position + offsetHeight, groundCheckRadius);
    }


    void Look()
    {
        lookY = Mathf.Clamp(lookY, minY, maxY);
        player.localEulerAngles = new Vector3(0, lookX, 0);
        cam.localEulerAngles = new Vector3(lookY, 0, 0);
    }

    void Move()
    {
        UpdateMoveDirectionFlat(inputDirection);
        animator.SetFloat("Speed", moveDirection.magnitude);
        if (isSlide)
        {
            characterController.Move(transform.forward * moveSpeed_Current * Time.deltaTime);
        }
        else if (isStick)
        {
        }
        else if (isMantle)
        {
            characterController.Move(mantleMoveDir * mantleSpeed * Time.deltaTime);
        }
        else
        {
            // characterController.Move( Quaternion.AngleAxis(transform.eulerAngles.y, transform.up) *moveDirection *
            //                           moveSpeed_Current * Time.deltaTime);
            characterController.Move(moveDirection * moveSpeed_Current * Time.deltaTime);
        }
    }

    void UpdateVelocity()
    {
        if (isSlide)
        {
            float slideToCrouchSpeedMultiplier = 0.85f;
            if (moveSpeed_Current <= moveSpeed_Default*moveSpeed_Multiplier * crouchSpeedMultiplier * slideToCrouchSpeedMultiplier)
            {
                OnSlide_End();
                Crouch();
            }

            //moveSpeed_Current = Mathf.Max(moveSpeed_Current - slideDecel * Time.deltaTime, 0f);

            moveSpeed_Current =
                slideCurve.Evaluate((Time.time - slideStartTime) / slideDuration) * slideSpeedMultiplier *
                moveSpeed_Default*moveSpeed_Multiplier;
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

    public void SetMoveSpeed_Current(float f)
    {
        Debug.Log($"overriding from {moveSpeed_Current} to {f}");
        moveSpeed_Current = f;
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


        inputDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        inputDirection = inputDirection;


        float newMoveSpeed = 0f;
        if (inputDirection.magnitude > 0.1f)
        {
            playerSoundScript.Set_Walk(isGrounded && !isSlide);
            // playerSoundScript.Set_Crouch(isCrouch);
            if (isCrouch)
            {
                newMoveSpeed = crouchSpeedMultiplier * moveSpeed_Default*moveSpeed_Multiplier;
            }
            else if (isSlide)
            {
            }
            else
            {
                newMoveSpeed = moveSpeed_Default*moveSpeed_Multiplier;
            }
        }
        else
        {
            playerSoundScript.Set_Walk(false);
            if (!isSlide)
            {
                newMoveSpeed = 0f;
            }
        }

        if (isGrounded && (Math.Abs(newMoveSpeed - moveSpeed_Target) > 0.001f))
        {
            SetMoveSpeed_Target(newMoveSpeed);
        }
        else if (!isGrounded && moveSpeed_Target == 0)
        {
            SetMoveSpeed_Target(newMoveSpeed);
        }
    }

    void UpdateMoveDirectionFlat(Vector3 newMoveDir)
    {
        newMoveDir = RelativeToFacing(newMoveDir);
        newMoveDir.y = 0;

        Vector3 temp = moveDirection;
        temp.y = 0;
        if (isGrounded)
        {
            temp = Vector3.Lerp(temp.normalized, newMoveDir.normalized, 1);
        }
        else
        {
            temp = Vector3.Lerp(temp.normalized, newMoveDir.normalized, inAirControl * Time.deltaTime);
        }

        temp.y = 0;

        moveDirection = temp;
    }

    private Vector3 RelativeToFacing(Vector3 newMoveDir)
    {
        return Quaternion.AngleAxis(transform.eulerAngles.y, transform.up) * newMoveDir;
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

        if (context.canceled)
        {
            if (IsBodySideTouch() && !isGrounded && isStick)
            {
                OnWallBounch();
            }

            if (isStick)
            {
                isStick = false;
                HandController.left.RemovePointer(stickHandPointer);
            }
        }
        else if (context.performed)
        {
            if (isCrouch)
            {
                UnCrouch();
            }
            else if (CanMantle())
            {
                OnMantle();
            }

            else if (!isGrounded && !isStick && IsBodySideTouch())
            {
                OnStick();
            }
            else
            {
                playerSoundScript.Set_Walk(false);
                //To jumping
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
                        ForceOverridesMoveDirection();
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
    }

    void OnMantle()
    {
        Debug.Log("OnMantle");

        StartCoroutine(MantleCoroutine());
    }

    IEnumerator MantleCoroutine()
    {
        isMantle = true;

        HandController.left.AddPointer(mantleHandPointer);
        mantleMoveDir = (Quaternion.AngleAxis(-mantleEuler, transform.right) * transform.forward).normalized;
        SetMantleHand();
        characterController.slopeLimit = slopLimit_Mantle;
        characterController.stepOffset = stepLimit_Mantle;
        jumpVelocity = 0;
        yield return new WaitForSeconds(mantleTime);
        characterController.slopeLimit = slopLimit_Default;
        characterController.stepOffset = stepLimit_Default;
        HandController.left.RemovePointer(mantleHandPointer);
        isMantle = false;
    }

    bool CanMantle()
    {
        if (!headVaultDetector.IsObstructed && frontWallDetector.IsObstructed)
        {
            return true;
            if (Physics.Raycast(GetPlayerCentre(), transform.forward, characterController.radius * 1.5f, jumpLayerMask))
            {
            }
        }

        return false;
    }

    private bool IsBodySideTouch()
    {
        // print(characterController.coll);

        if (sideDetector.IsObstructed)
        {
            return true;
        }
        //print("Not touch");

        return false;
    }

    void OnWallBounch()
    {
        print("Bounce");
        playerSoundScript.Play_Bounce();
        SetMoveSpeed_Current(bounceSpeedMult * moveSpeed_Default*moveSpeed_Multiplier);
        ForceOverridesMoveDirection();
        jumpVelocity = lookScript.GetFaceForward().y * bounceYMult * moveSpeed_Default*moveSpeed_Multiplier;
    }

    void OnStick()
    {
        isStick = true;
        jumpVelocity = 0f;
        FindStickHandPosition();
    }

    public void OnMelee(InputAction.CallbackContext callbackContext)
    {
        if (playerMelee)
        {
            if (callbackContext.performed)
            {
                if (playerMelee.CanMelee())
                {
                    gunDamageScript.PressFire(false);
                    gunDamageScript.PressADS(false);
                    animator.Play("Player_Melee");
                    playerMelee.Melee();
                }
            }
        }
        else
        {
            Debug.LogError("Missing Player Melee");
        }
    }

    void UpdateUIControlPrompts()
    {
        if (CanMantle())
        {
            playerUIScript.DisplayControlPrompt(ControlPromptType.Mantle);
        }
        else if (isStick)
        {
            playerUIScript.DisplayControlPrompt(ControlPromptType.Bounce);
        }
        else if (!isGrounded && !isStick && IsBodySideTouch())
        {
            playerUIScript.DisplayControlPrompt(ControlPromptType.Stick);
        }
        else
        {
            playerUIScript.CloseAllControlPrompt();
        }
    }

    private void SetMoveDirectionToFaceForward()
    {
        moveDirection = Vector3.Lerp(lookScript.GetFaceForward(), RelativeToFacing(inputDirection), .5f).normalized;
    }

    private void ForceOverridesMoveDirection()
    {
        moveDirection = RelativeToFacing(inputDirection);
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

            if (dashCharges > 0 && inputDirection.magnitude > 0)
            {
                /*
                dashRange = transform.TransformDirection(moveDirection) * (dashDistance * 100);
                controller.Move(dashRange * Time.deltaTime);
                */
                ForceOverridesMoveDirection();
                playerSoundScript.Play_Dash();
                dashCharges--;
                dashStart = Time.time;
                playerVolumeControllerScript.PlayLD();
                StartCoroutine(DashCoroutine());
                playerUIScript.UpdateDashDisplay(dashCharges);
            }
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
        }
        else if (context.canceled)
        {
            if (isSlide)
            {
                OnSlide_End();
            }
            else if (!isCrouch && isGrounded)
            {
                Crouch();
            }
            else
            {
                UnCrouch();
            }
        }
    }

    private void UnCrouch()
    {
        // print("UnCrouch");
        SetPlayerHeight(1);
        isCrouch = false;
        SetMoveSpeed_Target(moveSpeed_Default*moveSpeed_Multiplier*inputDirection.magnitude);
        playerSoundScript.Set_Crouch(isCrouch);
    }

    private void Crouch()
    {
        // print("Crouch");
        SetPlayerHeight(crouchHeightMultiplier);
        isCrouch = true;
        SetMoveSpeed_Target(moveSpeed_Default * crouchSpeedMultiplier*moveSpeed_Multiplier);
        playerSoundScript.Set_Crouch(isCrouch);
    }

    public void OnSlide(InputAction.CallbackContext context)
    {
        if (isGrounded && context.performed && inputDirection.z > 0.1f && moveSpeed_Current > moveSpeed_Default*moveSpeed_Multiplier * .5f)
        {
            // print("Slide pressed");
            OnSlide();
        }
    }

    public void OnSlide()
    {
        SetPlayerHeight(slideHeightMultiplier);
        SetMoveSpeed_Target(moveSpeed_Default*moveSpeed_Multiplier * slideSpeedMultiplier);
        isSlide = true;
        slideStartTime = Time.time;
        animator.SetBool("Slide", true);
        HandController.left.AddPointer(slideHandPointer);
        playerSoundScript.Set_Walk(false);
        playerSoundScript.Play_Slide();
    }

    public void OnSlide_End()
    {
        UnCrouch();
        isSlide = false;
        animator.SetBool("Slide", false);
        HandController.left.RemovePointer(slideHandPointer);
        playerSoundScript.Set_Walk(isGrounded && !isSlide && inputDirection.magnitude > 0.1f);
    }

    void UpdateSlideHandPosition()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(slideCastPoint.position, -transform.up, out raycastHit, height_Original,
            jumpLayerMask))
        {
            slideHandPointer.transform.position = raycastHit.point;
            //slideHandPointer.transform.forward = transform.forward;
            //slideHandPointer.transform.right = -raycastHit.normal;
        }
    }

    void FindStickHandPosition()
    {
        int slices = 8;
        RaycastHit[] raycastHits = new RaycastHit[slices];
        List<RaycastHit> castPosition = new List<RaycastHit>();
        float rotateAmount = 360f / slices;
        float castDistance = characterController.radius * 2f;


        for (int i = 0; i < slices; i++)
        {
            Debug.Log($"Cast Hand position {i}");

            if (Physics.Raycast(lookScript.GetHead(),
                Quaternion.AngleAxis(rotateAmount * i, transform.up) * transform.forward, out raycastHits[i],
                castDistance,
                jumpLayerMask))
            {
                castPosition.Add(raycastHits[i]);

                Debug.DrawLine(lookScript.GetHead(), raycastHits[i].point, Color.green, 3f);
            }
        }

        // if cast did not find a wall
        if (castPosition.Count == 0)
        {
            stickPoint = new Vector3();
            return;
        }

        //find closest point
        RaycastHit currentHit = castPosition[0];
        foreach (var v in castPosition)
        {
            if (Vector3.Distance(lookScript.GetHead(), v.point) <
                Vector3.Distance(lookScript.GetHead(), currentHit.point))
            {
                currentHit = v;
            }
        }

        //Recasting for the last cooldown
        if (Physics.Raycast(lookScript.GetHead(), -currentHit.normal, out handStickHit, castDistance,
            jumpLayerMask))
        {
            var transform1 = stickHandTransform.transform;

            stickPoint = handStickHit.point;
            //transform1.forward= Vector3.Cross(handStickHit.normal, transform1.up);;

            //transform1.Rotate(transform1.right,180f);

            HandController.left.AddPointer(stickHandPointer);
        }
        else
        {
            Debug.LogWarning("Failed to set stickPoint");
        }
    }

    void UpdateStickHandPosition()
    {
        if (!stickHandPointer)
        {
            Debug.LogWarning("Missing stick hand pointer");
            return;
        }

        if (stickPoint.magnitude > 0)
        {
            var transform1 = stickHandTransform.transform;
            transform1.position = stickPoint;
            //transform1.forward = lookScript.GetFaceForward();
            Vector3 temp = Vector3.Cross(handStickHit.normal, transform1.up);
            // temp = transform1.InverseTransformDirection(handStickHit.normal);
            // temp = Quaternion.Euler()
            //temp = Quaternion.AngleAxis(0f, temp) * temp;

            print(temp);

            transform1.rotation = Quaternion.LookRotation(temp);
            //transform1.Rotate(temp,180f);

            //transform1.right = -handStickHit.normal;
        }
    }

    void SetMantleHand()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(mantleHandCast.position, mantleHandCast.forward, out raycastHit, height_Original,
            jumpLayerMask))
        {
            mantleHandPos = raycastHit.point;
            //slideHandPointer.transform.forward = transform.forward;
            //slideHandPointer.transform.right = -raycastHit.normal;
        }
        else
        {
            mantleHandPos = new Vector3();
        }
    }

    void UpdateMantleHand()
    {
        if (mantleHandPos.magnitude > 0)
        {
            mantleHandPointer.transform.position = mantleHandPos;
        }
    }

    public void Shoot(InputAction.CallbackContext callbackContext)
    {
        if (disableControl)
        {
            return;
        }

        if (callbackContext.performed && playerMelee.CanMelee())
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

        if (callbackContext.performed && playerMelee.CanMelee())
        {
            gunDamageScript.PressADS(true);
        }
        else if (callbackContext.canceled)
        {
            gunDamageScript.PressADS(false);
        }
    }


    public void Reload(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            gunDamageScript.Reload();
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
            playerUIScript.CloseAllMenus();
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
        moveSpeed_Target = moveSpeed_Default*moveSpeed_Multiplier * dashMultiplier;
        moveSpeed_Current = moveSpeed_Target;
        yield return new WaitForSeconds(dashDuration);
        moveSpeed_Target = moveSpeed_Default*moveSpeed_Multiplier;
        moveSpeed_Current = moveSpeed_Target;
    }

    private void OnEnable()
    {
        // moveSpeed_Cap = moveSpeed_Default*moveSpeed_Multiplier;
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

    public Vector3 GetPlayerCentre()
    {
        return transform.position + characterController.center;
    }


    void UpdateJumpAndGravity()
    {
        bool wasGrounded = isGrounded;
        GroundCheck();
        HeadCheck();
        characterController.Move(new Vector3(0, jumpVelocity * Time.deltaTime, 0));
        animator.SetBool("Grounded", isGrounded);

        if (wasGrounded != isGrounded)
        {
            //playerSoundScript.Play_Jump();
            if (isGrounded)
            {
                //SetMoveSpeed_Target(moveSpeed_Default);
                playerSoundScript.Set_Walk(inputDirection.magnitude > 0.1f);
            }
        }

        if (!isGrounded && !isStick && !isMantle)
        {
            jumpVelocity += gravity * Time.deltaTime;
        }
        else if (isMantle)
        {
            jumpVelocity = 0f;
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

    void SetPlayerHeight(float m)
    {
        height_Target = height_Original * m;
        Vector3 localScale = playerHitBox.localScale;
        localScale.y = m;
        playerHitBox.localScale = localScale;
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

    public void AddMeleeHandPointer()
    {
        HandController.left.AddPointer(meleeHandPointer);
    }

    public void RemoveMeleeHandPointer()
    {
        HandController.left.RemovePointer(meleeHandPointer);
    }


    public void BoostStatsMoveSpeedMultiplier(float f)
    {
        moveSpeed_Multiplier += f;
        SetMoveSpeed_Target(moveSpeed_Default*moveSpeed_Multiplier*inputDirection.magnitude);
    }
}