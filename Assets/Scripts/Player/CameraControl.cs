using System.Globalization;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
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

    public NetworkVariable<float> serverX;
    public NetworkVariable<float> serverY;

    
    public AnticipatedNetworkTransform ANT;


    float xRotation;
    float yRotation;

    public delegate void LookDelegate();
    LookDelegate lookMode;
    private float interpolationSpeed = 40f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //lock the cursor to the center, hide it becuase first person
    public override void OnNetworkSpawn()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;


        lookMode = localViewMode;
    


        if (!IsOwner) // Other clients subscribe to changes to update remote player visuals
        {
            serverY.OnValueChanged += OnServerYawChanged;
            serverX.OnValueChanged += OnServerPitchChanged;
            lookMode = replicateViewMode;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lookMode();
    }

    
    void localViewMode()
    {

        //force this to update client side first
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        applyRot(xRotation, yRotation);

        SubmitRotationRequestRpc(xRotation, yRotation);
    }

    void applyRot (float xRotation, float yRotation)
    {
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        orientation.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerRoot.localRotation = Quaternion.Euler(0, yRotation, 0);
    }


    [Rpc(SendTo.Server)]
    private void SubmitRotationRequestRpc(float xRotation, float yRotation)
    {
        this.xRotation = xRotation;
        this.yRotation = yRotation;

        if (IsServer)
        {
            serverX.Value = xRotation;
            serverY.Value = yRotation;
        }
    }
    


    void replicateViewMode()
    {
        //transform.localRotation = 
        //    Quaternion.Slerp(transform.localRotation, 
        //    Quaternion.Euler(serverX.Value, 0f, 0f), 
        //    Time.deltaTime * interpolationSpeed);

        //orientation.localRotation =
        //    Quaternion.Slerp(playerRoot.localRotation,
        //    Quaternion.Euler(serverX.Value, 0f, 0f),
        //    Time.deltaTime * interpolationSpeed);

        //playerRoot.localRotation =
        //    Quaternion.Slerp(playerRoot.localRotation,
        //    Quaternion.Euler(0f, serverY.Value, 0f),
        //    Time.deltaTime * interpolationSpeed);

    }


    private void OnServerYawChanged(float previousValue, float newValue)
    {
        if (!IsOwner) // Only apply directly for remote players
        {
            playerRoot.localRotation = Quaternion.Euler(0, newValue, 0);
        }
        // Owning client: Implement reconciliation logic here if there's a mismatch
        // with its predicted 'currentYaw'. This might involve smoothly interpolating
        // or snapping if the difference is too large.
    }

    private void OnServerPitchChanged(float previousValue, float newValue)
    {
        if (!IsOwner) // Only apply directly for remote players
        {
            // Apply to the camera holder or relevant part of the remote player's model
            transform.localRotation = Quaternion.Euler(newValue, 0, 0);
            orientation.localRotation = Quaternion.Euler(newValue, 0, 0);
        }
        // Owning client: Reconciliation for pitch.
    }

    // Example of how the owning client might apply server state in its Update
    //void Update()
    //{
    //    if (IsOwner)
    //    {
    //        // ... (input handling and local application) ...
    //        SubmitRotationRequestRpc(xRotation, yRotation); // Send to server
    //    }
    //    else // For remote clients
    //    {
    //        // Smoothly interpolate to the server-authoritative rotation
 
    //    }
    //}
}
