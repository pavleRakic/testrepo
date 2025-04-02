using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform followTarget;
    [SerializeField] private Vector3 offset; 
    [SerializeField] private float height;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float maxZoomIn= 0.5f;
    [SerializeField] private float maxZoomOut = 1.5f;


    private Vector3 mousePosition;

    private Quaternion rotation;
    private float x=0;
    private float y=0;
    private bool lockedMouse = false;
    public float scroll = 1;

    void Start()
    {
        
    }

    void Update()
    {
        x += Input.GetAxis("Mouse X");
        y += Input.GetAxis("Mouse Y");

        scroll -= Input.GetAxis("Mouse ScrollWheel");
        if(scroll > maxZoomOut)
            scroll = maxZoomOut;
        if(scroll <maxZoomIn)
            scroll = maxZoomIn;
    }


    void LateUpdate()
    {
        if(Input.GetMouseButton(1) || Input.GetMouseButton(0) )
        {
            rotation = Quaternion.Euler(-1*y,x,0);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            lockedMouse = true;
        }

        if(!Input.GetMouseButton(1) && !Input.GetMouseButton(0) && lockedMouse == true)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            lockedMouse = false;

        }
            transform.rotation = rotation;
            
            transform.position = followTarget.position - rotation*offset*scroll + Vector3.up * height;
            
            //transform.rotation = Quaternion.Slerp(transform.rotation,rotation, Time.deltaTime*rotationSpeed);
    }
}
