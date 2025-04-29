using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(CharacterController))]
public class Climber : MonoBehaviour
{
    private CharacterController characterController;
    public static XRController climbingHand; // Using XRController instead of XRBaseController

    [Tooltip("Reference to your movement script")]
    public MonoBehaviour movementScript;

    [Tooltip("Climbing speed multiplier")]
    public float climbSpeed = 1f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Missing CharacterController on " + gameObject.name);
        }
    }

    private void FixedUpdate()
    {
        if (climbingHand != null)
        {
            if (movementScript != null) movementScript.enabled = false;
            Climb();
        }
        else
        {
            if (movementScript != null) movementScript.enabled = true;
        }
    }

    private void Climb()
    {
        if (climbingHand == null || characterController == null) return;

        // Get the controller's device
        InputDevice device = InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode);

        if (device.isValid && device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity))
        {
            Vector3 movement = transform.rotation * -velocity * Time.fixedDeltaTime * climbSpeed;
            characterController.Move(movement);
        }
    }
}