
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

    Vector3 jumped;

    float moveSpeed;
    [SerializeField] float moveSpeed_Default;

    CharacterController controller;
    Vector3 moveDirection;
    bool jump;
    bool run;

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

    public PlayerGunDamageScript GunDamageScript { get => gunDamageScript; set => gunDamageScript = value; }
    public PlayerInventorySystemScript PlayerInventorySystemScript { get => playerInventorySystemScript; set => playerInventorySystemScript = value; }
    public PlayerInterationScript PlayerInterationScript { get => playerInterationScript; set => playerInterationScript = value; }
    public bool DisableControl { get => disableControl; set => disableControl = value; }



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
                CameraTilt();

            }
            if (!controller.isGrounded)
            {
                //print("Adding gravity");
                //controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
                jumped.y -= gravity * Time.deltaTime;
            }
        }

        if (dashCharges<dashCharges_Max&& Time.time > dashStart + dashCooldown)
        {
            dashCharges++;
            dashStart = Time.time;

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

    public void SetControlLock(bool b)
    {
        disableControl = !b;
        lookScript.LookLock = !b;
        if (b)
        {

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

    public void OnLook(InputAction.CallbackContext context)
    {
        if (disableControl)
        {
            return;
        }
        //lookX += context.ReadValue<Vector2>().x * sensitivityX * Time.deltaTime;
        //lookY -= context.ReadValue<Vector2>().y * sensitivityY * Time.deltaTime;
        lookScript.LookMouse(context);
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
            if (controller.isGrounded)
            {
                canDoubleJumped = true;
                jumped = new Vector3(0f, jumpSpeed, 0f);
            }
            else
            {
                if (canDoubleJumped)
                {
                    jumped = new Vector3(0f, doubleJumpSpeed, 0f);
                    canDoubleJumped = false;
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
            if (dashCharges>0 && moveDirection.magnitude > 0)
            {
                /*
                dashRange = transform.TransformDirection(moveDirection) * (dashDistance * 100);
                controller.Move(dashRange * Time.deltaTime);
                */
                dashCharges--;
                dashStart = Time.time;
                StartCoroutine(DashCoroutine());
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
            }
            else
            {
                playerInterationScript.useInteractable();
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


}