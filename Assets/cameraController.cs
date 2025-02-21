using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class cameraController : MonoBehaviour
{
    public float mouseSensitivity = 20f; // Adjust this for speed
    public float smoothTime = 0.05f;
    public Transform playerBody;

    private float xRotation = 0f;
    private float smoothX, smoothY;
    private float xVelocity, yVelocity;  // For smoothing

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center

        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        // New Input System: Get Mouse Movement
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // Smooth mouse movement
        smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref xVelocity, smoothTime);
        smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref yVelocity, smoothTime);

        // Rotate camera up/down (invert Y-axis to match real head movement)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent flipping over

        //prevent looking down 
        // ?? Prevent camera from looking down instantly at start
        if (Time.timeSinceLevelLoad > 0.1f) // Small delay after game starts
        {
            xRotation -= smoothY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }

        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Camera up/down

        playerBody.Rotate(Vector3.up * mouseX); // Rotate player left/right
    }

}
