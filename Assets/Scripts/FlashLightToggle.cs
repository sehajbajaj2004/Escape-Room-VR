using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class FlashlightToggle : MonoBehaviour
{
    [Header("Flashlight Light Object (Disabled at start)")]
    public GameObject lightObject;

    [Header("XR Grab Interactable")]
    public XRGrabInteractable grabInteractable;

    [Header("Controller Input")]
    public InputActionReference aButtonAction; // Assign A button (PrimaryButton)

    private bool isGrabbed = false;
    private bool lightOn = false;

    private void OnEnable()
    {
        aButtonAction.action.Enable();
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        aButtonAction.action.Disable();
        grabInteractable.selectEntered.RemoveListener(OnGrabbed);
        grabInteractable.selectExited.RemoveListener(OnReleased);
    }

    private void Start()
    {
        if (lightObject != null)
            lightObject.SetActive(false);   // start OFF
    }

    private void Update()
    {
        if (!isGrabbed)
            return;

        // Check if A button was pressed this frame
        if (aButtonAction.action.WasPerformedThisFrame())
        {
            ToggleLight();
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    private void ToggleLight()
    {
        lightOn = !lightOn;

        if (lightObject != null)
            lightObject.SetActive(lightOn);
    }
}
