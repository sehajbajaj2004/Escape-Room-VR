using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class Climber : MonoBehaviour
{
    [Header("References")]
    public static XRBaseController climbingHand;
    public Transform cameraOffset; // Reference to your XR Origin's CameraOffset
    public ContinuousMoveProviderBase movementScript;

    [Header("Climbing Settings")]
    public float climbSpeed = 1.5f;
    public float maxArmLength = 0.7f; // Maximum reach distance
    public float gravity = 9.81f;

    private CharacterController character;
    private Vector3 handAnchorPosition;
    private Vector3 lastHandPosition;
    private bool isClimbing;
    private float initialCameraHeight;

    void Start()
    {
        character = GetComponent<CharacterController>();
        initialCameraHeight = cameraOffset.localPosition.y;

        if (!cameraOffset)
            cameraOffset = GetComponentInChildren<Camera>().transform.parent;
    }

    void Update()
    {
        if (isClimbing)
        {
            ClimbMovement();
            ApplyArmStretch();
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            ApplyGravity();
        }
    }

    public void StartClimbing(XRBaseController controller)
    {
        climbingHand = controller;
        handAnchorPosition = controller.transform.position;
        lastHandPosition = handAnchorPosition;
        isClimbing = true;

        if (movementScript)
            movementScript.enabled = false;
    }

    public void StopClimbing()
    {
        isClimbing = false;
        climbingHand = null;

        if (movementScript)
            movementScript.enabled = true;

        // Reset camera offset
        cameraOffset.localPosition = new Vector3(
            cameraOffset.localPosition.x,
            initialCameraHeight,
            cameraOffset.localPosition.z
        );
    }

    private void ClimbMovement()
    {
        if (climbingHand == null) return;

        Vector3 handPositionDelta = climbingHand.transform.position - lastHandPosition;
        Vector3 movement = -handPositionDelta * climbSpeed;

        character.Move(transform.rotation * movement);
        lastHandPosition = climbingHand.transform.position;
    }

    private void ApplyArmStretch()
    {
        if (climbingHand == null) return;

        // Calculate arm stretch (how far hand moved from initial grab point)
        float stretchAmount = Vector3.Distance(climbingHand.transform.position, handAnchorPosition);
        float normalizedStretch = Mathf.Clamp01(stretchAmount / maxArmLength);

        // Lower camera offset based on stretch
        float newHeight = initialCameraHeight - (normalizedStretch * 0.3f); // Adjust 0.3f for desired effect
        cameraOffset.localPosition = new Vector3(
            cameraOffset.localPosition.x,
            newHeight,
            cameraOffset.localPosition.z
        );
    }

    private void ApplyGravity()
    {
        if (!character.isGrounded)
        {
            character.Move(Vector3.down * gravity * Time.fixedDeltaTime);
        }
    }
}