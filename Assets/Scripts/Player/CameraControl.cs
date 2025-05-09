using UnityEngine;
using UnityEngine.InputSystem;


//largely stolen from dave/gamedevelopment yt channel (thanks!)
//also sort of adapted from the lukeskt's implementation
public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset _actions;

    public float sensX;
    public float sensY;

    public Transform orientation;

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
    void Update()
    {
        //float mouseX = Input.GetAxisRaw("")
    }
}
