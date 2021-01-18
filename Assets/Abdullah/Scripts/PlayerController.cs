
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float gravity = -9.81f;
    [SerializeField] public float jumpSpeed;

    [SerializeField] public int walkSpeed;
    [SerializeField] public int runSpeed;

    [SerializeField] public float sensitivityX;
    [SerializeField] public float sensitivityY;

    [SerializeField] public float minY = -60f;
    [SerializeField] public float maxY = 60f;

    [SerializeField] public float tilt = 20;

    Vector3 jumped;

    int moveSpeed;

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



    // Start is called before the first frame update
    void Start()
    {
        tilted = false;
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        player = transform;
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Look();
        CameraTilt();
        jumped.y -= gravity * Time.deltaTime;
    }

    void Jump() {

        if (controller.isGrounded) {
            jumped = new Vector3(0f,jumpSpeed,0f);
        }       
        
    }

    void CameraTilt() {
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
        if (run)
        {
            moveSpeed = runSpeed;
        }

        if (!run)
        {
            moveSpeed = walkSpeed;
        }

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        controller.Move(jumped * Time.deltaTime);
        
    }

    
    
    public void OnMove(InputAction.CallbackContext context)
    {
        isMoving = context.ReadValue<Vector2>();
        moveDirection = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
        moveDirection = transform.TransformDirection(moveDirection);
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
        lookX += context.ReadValue<Vector2>().x * sensitivityX * Time.deltaTime;
        lookY -= context.ReadValue<Vector2>().y * sensitivityY * Time.deltaTime;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jump = context.performed;
        if (jump)
        {
            Jump();
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        run = context.performed;
    }

}