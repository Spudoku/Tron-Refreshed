using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


//largely stolen from dave/gamedevelopment yt channel (thanks!)
//essentially 

public class CameraControl : NetworkBehaviour
{

    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform playerRoot;

    float xRotation;
    float yRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //lock the cursor to the center, hide it becuase first person
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsOwner) return;
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
   
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerRoot.localRotation = Quaternion.Euler(0, yRotation, 0);
        orientation.localRotation = Quaternion.Euler(xRotation, 0, 0);

    }
}
