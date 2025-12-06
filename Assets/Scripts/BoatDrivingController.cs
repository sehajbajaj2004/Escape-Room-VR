using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Content.Interaction;

public class BoatDrivingController : MonoBehaviour
{
    [Header("Steering Wheel (XRKnob)")]
    public XRKnob steeringWheel;

    [Header("Input Actions")]
    public InputActionReference leftTrigger;
    public InputActionReference rightTrigger;

    [Header("Boat Movement Settings")]
    public float maxForwardSpeed = 3f;
    public float accelerationRate = 2f;
    public float decelerationRate = 1.5f;

    [Header("Steering Settings")]
    public float turnStrength = 60f;

    [Header("Engine Sound")]
    public AudioSource engineAudio;

    private float currentSpeed = 0f;
    private Transform xrOrigin;

    void Start()
    {
        xrOrigin = this.transform;

        leftTrigger.action.Enable();
        rightTrigger.action.Enable();

        if (engineAudio != null)
        {
            engineAudio.loop = true;
            engineAudio.Stop();
        }
    }

    void Update()
    {
        HandleAcceleration();
        HandleSteering();
        HandleEngineSound();

        xrOrigin.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
    }

    private void HandleAcceleration()
    {
        bool leftHeld = leftTrigger.action.IsPressed();
        bool rightHeld = rightTrigger.action.IsPressed();

        if (leftHeld || rightHeld)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, maxForwardSpeed, Time.deltaTime * accelerationRate);
        }
        else
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime * decelerationRate);
        }
    }

    private void HandleSteering()
    {
        if (currentSpeed < 0.05f)
            return;

        if (steeringWheel == null)
            return;

        float wheelValue = (steeringWheel.value - 0.5f) * 2f;
        float turnAmount = wheelValue * turnStrength * Time.deltaTime;

        xrOrigin.Rotate(0, turnAmount, 0);
    }

    private void HandleEngineSound()
    {
        if (engineAudio == null)
            return;

        bool leftHeld = leftTrigger.action.IsPressed();
        bool rightHeld = rightTrigger.action.IsPressed();

        // Play while holding trigger
        if (leftHeld || rightHeld)
        {
            if (!engineAudio.isPlaying)
                engineAudio.Play();
        }
        else
        {
            // Stop instantly when trigger is released
            if (engineAudio.isPlaying)
                engineAudio.Stop();
        }
    }
}
