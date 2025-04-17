using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
     [Header("Movement settings")]
     [Space(10)]


     [Header("Walk settings")]
    [SerializeField] private float timeToFullWalkSpeed = 1f;
    [SerializeField] private float maxWalkSpeed = 20;
    private float walkAccelerationRate;
    [Space(10)]


     [Header("Sprint settings")]
    [SerializeField] private float timeToFullSprintSpeed = 0.5f;
    [SerializeField] private float maxSprintSpeed = 40;
    private float sprintAccelerationRate;
    [Space(10)]


    [Header("Charge settings")]
    [SerializeField] private float timeToFullChargeSpeed = 0.5f;
    [SerializeField] private float maxChargeSpeed = 60;
    [SerializeField] private float maxRotationSpeed = 10f;
    private float chargeAccelerationRate;
    [Space(10)]


    [Header("Jump settings")]
    [SerializeField] float timeToPeak = 2;
    [SerializeField] private float jumpHeight = 10; 
    public float groundDistance = 0.2f;  
    public LayerMask groundMask;
    [Space(10)]



    private float rotationSpeed = 10f;
    
    private float currentSpeed = 0;
    private float elapsedTime;
    private Vector3 previousPosition;
    private bool isGrounded;

    [SerializeField] private Transform cameraTransform;

    private bool jumpCompleted;
     float gravity = 10;
    private float initialVelocity = 0;

    private float velocity = 0;
    [SerializeField] private float cameraRotationSpeed= 150;
    [SerializeField] private CapsuleCollider childCapsule;


    void Start()
    {
        walkAccelerationRate = maxWalkSpeed/timeToFullWalkSpeed;
        sprintAccelerationRate = maxSprintSpeed/timeToFullSprintSpeed;
        chargeAccelerationRate = maxChargeSpeed/timeToFullChargeSpeed;
        isGrounded = true;
        previousPosition = transform.position;

        
        if (childCapsule == null) {
        childCapsule = GetComponentInChildren<CapsuleCollider>();
    }
    }

    void Update()
    {
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 currentPosition = transform.position;
        Vector3 inputVector = new Vector3(0,0,0);
        gravity = (-2*jumpHeight)/(timeToPeak*timeToPeak);
        initialVelocity = (2*jumpHeight)/timeToPeak;


        
        // Ground detection and falltrough correction
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundDistance, groundMask);
        if (Physics.Linecast(previousPosition, currentPosition, out RaycastHit hit, groundMask))
        {
            isGrounded = true;
            Debug.DrawLine(previousPosition, currentPosition, Color.green, 0.1f);
            transform.position = hit.point;
        }

        
        // Get input direction, rotate character
        if(Input.GetKey(KeyCode.W) && Input.GetMouseButton(1))
        {
           inputVector = cameraForward;
        }
        else if(Input.GetKey(KeyCode.W))
        {
            inputVector += transform.forward;
        }
        if(Input.GetKey(KeyCode.S) && Input.GetMouseButton(1))
        {
           inputVector = -cameraForward;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            inputVector -= transform.forward;
        }
        if(Input.GetKey(KeyCode.E))
        {
            inputVector += transform.right;
        }
        if(Input.GetKey(KeyCode.Q))
        {
            inputVector -= transform.right;
        }
        if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, cameraRotationSpeed * Time.deltaTime, 0);
        }
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -cameraRotationSpeed * Time.deltaTime, 0);
        }



        // Changing the speed
        bool isMoving = inputVector.magnitude > 0;
        if(isMoving)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed += sprintAccelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxSprintSpeed);
                rotationSpeed = maxRotationSpeed;
            }
            else if(Input.GetKey(KeyCode.C) && currentSpeed >= maxSprintSpeed-5)
            {
                currentSpeed += chargeAccelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxChargeSpeed);
                rotationSpeed = 4;
            }
            else
            {
                currentSpeed += walkAccelerationRate * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, maxWalkSpeed);
                rotationSpeed = maxRotationSpeed;
            }
        }
        else
        {
            currentSpeed = 0;
            rotationSpeed = maxRotationSpeed;
        }
        inputVector = inputVector.normalized;
        inputVector = inputVector * Time.deltaTime * currentSpeed;




        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity = initialVelocity;
            isGrounded = false;
        }
        else if(!isGrounded)
        {
            velocity += gravity*Time.deltaTime;
        }
        else
        {
            velocity = 0;
        }
        inputVector.y = velocity*Time.deltaTime;

        
        // Rotate character
        Quaternion cameraRotation = cameraTransform.rotation;
        cameraRotation.x =0;
        cameraRotation.z = 0;
        if(Input.GetMouseButton(1))
        {
           // transform.forward = Vector3.Slerp(transform.forward, -normCameraDirection, Time.deltaTime*rotationSpeed);
            transform.rotation = cameraRotation;
        }

        
               Vector3 moveDirection = inputVector;
    float moveDistance = currentSpeed*Time.deltaTime;
    /*
    // Check for collisions before moving
    if (Physics.CapsuleCast(
        childCapsule.transform.position + Vector3.up * 0.5f,  // Bottom sphere
        childCapsule.transform.position - Vector3.up * 0.5f,  // Top sphere
        childCapsule.GetComponent<CapsuleCollider>().radius,
        moveDirection,
        out RaycastHit hitS,
        moveDistance))
    {
        // Calculate slide direction
        Vector3 slideDirection = Vector3.ProjectOnPlane(moveDirection, hitS.normal);
        
        // Try moving along the surface
        if (slideDirection.magnitude > 0.01f) {
            transform.position += slideDirection.normalized * moveDistance;
        }
    }
    else {
        // No collision - normal movement
        //transform.position += moveDirection * moveDistance;
        transform.position += inputVector;

    }*/
       // Move Character
        transform.position += inputVector;
        


        previousPosition = currentPosition;
    }
}
