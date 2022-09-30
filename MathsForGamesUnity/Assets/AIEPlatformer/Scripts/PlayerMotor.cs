using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private CharacterController motor;

    [Header("Movement Options")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float sprintStam = 5f;
    public float MaxSprintStam
    {
        get { return sprintStam;  }
    }
    private float curSprintStam = 5f;
    public float SprintStam
    {
        get { return curSprintStam; }
    }
    private Vector3 moveWish;
    [SerializeField] private float sprintSpeed = 10f; 
    private bool sprintWish;
    public bool Sprinting
    {
        get { return sprintWish; }
    }
    private Vector3 respawnPos;

    [Header("Jumping Options")]
    private bool jumpWish;
    private float yVelocity;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private int maxJumps = 2;
    private int jumpCounter = 0;
    //for UI scripts to reference
    public int JumpCount
    {
        get { return jumpCounter; }
    }

    [Header("Rotation Options")]
    private Vector3 desiredForward;
    [SerializeField] float rotationSpeed = 720.0f;

    public bool hasKey = false;


    private void Start()
    {
        respawnPos = transform.position;
    }


    // Update is called once per frame
    private void Update()
    {
        moveWish = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        jumpWish = jumpWish || Input.GetButtonDown("Jump");
        sprintWish = sprintWish || (Input.GetButton("Fire3") && curSprintStam > 0);

        moveWish = Vector3.ClampMagnitude(moveWish, 1f);

        //if player drops below a certain heigh, respawn them
        if (transform.position.y <= -2)
        {
            motor.enabled = false;
            gameObject.transform.position = respawnPos;
            motor.enabled = true;
        }

    }

    private void FixedUpdate()
    {
        //combine forces
        Vector3 baseMove = moveWish * (moveSpeed);
        yVelocity += Physics.gravity.y * Time.deltaTime;

        if (motor.isGrounded)
        {
            yVelocity = Physics.gravity.y * Time.deltaTime;
            jumpCounter = 0;
        }

        if (jumpWish && jumpCounter < maxJumps)
        {
            jumpWish = false;

            if(motor.isGrounded || jumpCounter < maxJumps)
            {
                yVelocity = jumpForce;
                jumpCounter++;
            }
        }

        if (sprintWish)
        {
            baseMove = moveWish * sprintSpeed;
            curSprintStam -= Time.deltaTime;
            if(curSprintStam <= 0 || !Input.GetButton("Fire3"))
            {
                sprintWish = false;
            }
        }
        else
        {
            if (curSprintStam < 5.0f)
            {
                curSprintStam += (Time.deltaTime / 2);
            }
            else
            {
                curSprintStam = 5.0f;
            }
        }

        //reorient the movement onto the current ground normal
        if(Physics.Raycast( transform.position,                                 //location of the start of the ray
                            Vector3.down,                                       //direction to shoot the ray in  
                            out RaycastHit hit,                                 //data about the thing that the ray hit
                            2f,                                                 //how far (aka magnitude of the ray)
                            groundMask,                                         //which types of objects to hit?
                            QueryTriggerInteraction.Ignore))                    //whether we should include or ignore triggers
        {
            //Debug.Log("hit");
            baseMove = Vector3.ProjectOnPlane(baseMove, hit.normal);
        }

        //apply movement to the motor
        motor.Move(baseMove * Time.deltaTime);
        motor.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);

        //check if the player IS moving
        if (baseMove.magnitude != 0.0f)
        {
            //only rotates WHILE moving
            //if so, store the rotation towards the players desired movement direction
            //Quaternion toRotation = Quaternion.LookRotation(moveWish, Vector3.up);
            //then rotate towards that rotation
            //transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);

            //cache desired direction
            desiredForward = baseMove.normalized;
        }

        //move in the desired direction
        transform.forward = Vector3.RotateTowards(transform.forward, desiredForward, rotationSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
    }
}
