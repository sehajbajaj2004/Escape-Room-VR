using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class GasTankSprayController : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem sprayParticles;
    public XRGrabInteractable grabInteractable;

    [Header("Input Actions (Trigger Buttons)")]
    public InputActionProperty leftTrigger;
    public InputActionProperty rightTrigger;

    private bool isGrabbed = false;
    private XRBaseInteractor grabbingHand = null;

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void Update()
    {
        if (!isGrabbed || grabbingHand == null)
        {
            if (sprayParticles.isPlaying)
                sprayParticles.Stop();
            return;
        }

        bool triggerPressed = false;

        // Identify whether left or right hand is grabbing the object
        if (grabbingHand.CompareTag("LeftHand"))
        {
            triggerPressed = leftTrigger.action.ReadValue<float>() > 0.1f;
        }
        else if (grabbingHand.CompareTag("RightHand"))
        {
            triggerPressed = rightTrigger.action.ReadValue<float>() > 0.1f;
        }

        // Play or stop particle based on trigger input
        if (triggerPressed)
        {
            if (!sprayParticles.isPlaying)
                sprayParticles.Play();
        }
        else
        {
            if (sprayParticles.isPlaying)
                sprayParticles.Stop();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        grabbingHand = args.interactorObject.transform.GetComponent<XRBaseInteractor>();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        grabbingHand = null;

        if (sprayParticles.isPlaying)
            sprayParticles.Stop();
    }
}
