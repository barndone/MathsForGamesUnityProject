using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private CharacterController motor;

    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private LayerMask groundMask = ~0;

    [SerializeField] private float sprintStam = 5f;
    private float curSprintStam = 5f;


    private Vector3 moveWish;
    private bool jumpWish;
    private bool sprintWish;
    private float yVelocity;

    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float rotationSpeed = 8f;

    // Update is called once per frame
    private void Update()
    {
        moveWish = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        jumpWish = jumpWish || Input.GetButtonDown("Jump");
        sprintWish = sprintWish || (Input.GetButton("Fire3") && curSprintStam > 0);
    }

    private void FixedUpdate()
    {
        //combine forces
        Vector3 baseMove = moveWish * (moveSpeed);
        yVelocity += Physics.gravity.y * Time.deltaTime;

        if (motor.isGrounded)
        {
            yVelocity = Physics.gravity.y * Time.deltaTime;
        }

        if (jumpWish)
        {
            jumpWish = false;

            if(motor.isGrounded)
            {
                yVelocity = jumpForce;
            }
        }

        if (sprintWish)
        {
            baseMove = moveWish * sprintSpeed;
            curSprintStam -= Time.deltaTime;
            if(curSprintStam <= 0)
            {
                sprintWish = false;
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
            Debug.Log("hit");
            baseMove = Vector3.ProjectOnPlane(baseMove, hit.normal);
        }

        //apply movement to the motor
        motor.Move(baseMove * Time.deltaTime);
        motor.Move(new Vector3(0, yVelocity, 0) * Time.deltaTime);

        //check if the player IS moving
        if (baseMove != Vector3.zero)
        {
            //if so, store the rotation towards the players desired movement direction
            Quaternion toRotation = Quaternion.LookRotation(moveWish, Vector3.up);
            //then rotate towards that rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
