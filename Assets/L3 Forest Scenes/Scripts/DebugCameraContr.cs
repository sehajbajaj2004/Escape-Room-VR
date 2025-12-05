using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A free-look camera controller allowing WASD movement and mouse rotation (pitch and yaw).
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed for WASD movement.")]
    public float movementSpeed = 5.0f;
    
    [Tooltip("Speed multiplier when holding Shift.")]
    public float fastSpeedMultiplier = 2.0f;

    [Header("Look Settings")]
    [Tooltip("Sensitivity for mouse input.")]
    public float mouseSensitivity = 150.0f;
    
    [Tooltip("Clamps vertical (pitch) rotation to prevent camera flip.")]
    public float pitchLimit = 90.0f; // Maximum angle up or down

    // Internal state variables for mouse look
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Lock the cursor to the center of the screen and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Initialize rotations based on current transform orientation
        Vector3 rot = transform.localRotation.eulerAngles;
        rotationY = rot.y;
        rotationX = rot.x;
    }

    void Update()
    {
        HandleKeyboardMovement();
        HandleMouseLook();
        
        // Optional: Pressing Escape unlocks the cursor for testing purposes
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// Handles camera movement using WASD keys.
    /// </summary>
    private void HandleKeyboardMovement()
    {
        // 1. Determine speed based on Shift key
        float currentSpeed = movementSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= fastSpeedMultiplier;
        }

        // 2. Get input axis values (range -1 to 1)
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right
        float vertical = Input.GetAxis("Vertical");   // W/S or Up/Down

        // 3. Calculate the movement vector relative to the camera's orientation
        // transform.forward is the direction the camera is currently looking
        // transform.right is the camera's right direction
        Vector3 moveDirection = transform.forward * vertical + transform.right * horizontal;
        
        // 4. Apply movement
        transform.position += moveDirection * currentSpeed * Time.deltaTime;
        
        // Optional: Handle elevation with Q and E
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position -= Vector3.up * currentSpeed * Time.deltaTime; // Move down
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Vector3.up * currentSpeed * Time.deltaTime; // Move up
        }
    }

    /// <summary>
    /// Handles camera rotation (pitch and yaw) based on mouse input.
    /// </summary>
    private void HandleMouseLook()
    {
        // Get raw mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Yaw (Y-axis rotation - horizontal rotation)
        rotationY += mouseX;
        
        // Pitch (X-axis rotation - vertical rotation)
        // Note: We subtract mouseY because moving the mouse UP should rotate the camera DOWN (negative pitch)
        rotationX -= mouseY;
        
        // Clamp the vertical rotation (pitch) so the camera doesn't flip over
        rotationX = Mathf.Clamp(rotationX, -pitchLimit, pitchLimit);

        // Apply the calculated rotations
        // We set the full rotation directly using quaternions (recommended for rotation)
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }
}