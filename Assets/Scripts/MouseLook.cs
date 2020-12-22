using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity;
    float xRotation = 0f;
    public static bool cursorLocked = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

        ToggleCursor();
    }

    void ToggleCursor()
    {
        if(cursorLocked){
            Cursor.lockState = CursorLockMode.Locked;
            if(Input.GetKeyDown(KeyCode.Escape)){
                cursorLocked = false;
            }
        }
        else{
            Cursor.lockState = CursorLockMode.None;
            if(Input.GetKeyDown(KeyCode.Escape)){
                cursorLocked = true;
            }
        }
    }
}
