using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

public class PlayerController : MonoBehaviour
{
    private Vector2 inputVector = new Vector2(0, 0);
    public float moveSpeed = 1f;
    public float moveSpeedMax = 10f;
    private Vector3 moveDr;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 rotDir;
    [SerializeField] bool grounded;
    public float jumpStrength;
    [Range(0f, 1.1f)]
    public float jumpMovementMultiplier = 0.5f;
    [SerializeField]LayerMask layerMask;


    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Component")]
    private Rigidbody rb;
    public Camera camera1;
    public Animator animator;

    public bool Grounded { get => grounded; set => grounded = value; }

    private void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        camera1 = FindObjectOfType<Camera>();
        Grounded = true;
      

    }


    public void Move(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();

        moveDr = new Vector3(inputVector.x, 0, inputVector.y);
    }

    public bool Jump(InputAction.CallbackContext context)
    {

        if (context.performed && Grounded) {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * (jumpStrength*1000));
            //animator.SetBool("isFalling", true);
            Grounded = false;
            //animator.SetBool("Grounded", false);
        }
        return Grounded;
         
    }

    private void FixedUpdate()
    {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y >0 && !Grounded ) {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (moveDr.magnitude >= 0.1f)
        {
            //RotateWithCamera();
            float gravity = rb.velocity.y;
            float tempMultiplier = 1f;
            if (!grounded)
            {
                tempMultiplier = jumpMovementMultiplier;
            }
            //rb.velocity = (rotDir.normalized * moveSpeed* tempMultiplier);
            rb.velocity = (transform.forward * moveSpeed* tempMultiplier);
            if (rb.velocity.magnitude >= moveSpeedMax * tempMultiplier)
            {
                rb.velocity = rb.velocity.normalized * moveSpeedMax * tempMultiplier;
            }
            rb.velocity += new Vector3(0, gravity, 0);
        }
        //animator.SetFloat("Speed", rb.velocity.magnitude);
        //animator.transform.position = transform.position;
        //animator.transform.rotation = transform.rotation;

        //Anson: Added to update falling animation
        Grounded = Physics.Raycast(transform.position + Vector3.up, Vector3.down, 1.6f, layerMask);
        //animator.SetBool("Grounded", Grounded);
    }

    // dont delete this its my pride and joy :^D

    /* private void OnCollisionEnter(Collision collision)
     {
         if (collision.collider.CompareTag("Floor")) {
             grounded = true;

         }
         else if (!collision.collider.CompareTag("Floor") &&  rb.velocity.y <= 0.01 && rb.velocity.y >= -0.01) {
             grounded = true;

         }

     }

     private void OnCollisionExit(Collision collision)
     {
             grounded = false;

          if (collision.collider.CompareTag("Enviroment") && rb.velocity.y <= 0.01 && rb.velocity.y >= -0.01)
         {
             grounded = true;
         }
     }
     */


    private void RotateWithCamera()
    {

        float targetAngle = Mathf.Atan2(moveDr.x, moveDr.z) * Mathf.Rad2Deg + camera1.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        rotDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;


    }

    public void RotateWithCamera_Force()
    {
        float targetAngle = Mathf.Atan2(moveDr.x, moveDr.z) * Mathf.Rad2Deg + camera1.transform.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

        rotDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

    }
}
