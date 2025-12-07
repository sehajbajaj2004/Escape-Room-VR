using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CrystalLightActivator : MonoBehaviour
{
    [Header("Crystal Identification")]
    [Tooltip("The specific Grab Interactable object (the 'Crystal') required to activate the light.")]
    [SerializeField]
    private XRGrabInteractable requiredCrystal;

    [Header("Light Control")]
    [Tooltip("The Light component to be turned ON when the correct crystal is socketed.")]
    [SerializeField]
    private Light controlledLight;

    [Tooltip("Check this if the light should start off and only turn on when the correct crystal is placed.")]
    [SerializeField]
    private bool lightStartsOff = true;

    // The Socket Interactable component on this GameObject
    private XRSocketInteractor socketInteractor;

    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();

        // Ensure we have a socket and a light
        if (socketInteractor == null)
        {
            Debug.LogError("CrystalLightActivator requires an XRSocketInteractor on the same GameObject.", this);
            return;
        }

        if (controlledLight == null)
        {
            Debug.LogError("Controlled Light is not assigned in the Inspector.", this);
            return;
        }

        // Set initial light state
        if (lightStartsOff)
        {
            controlledLight.enabled = false;
        }
        else
        {
            // If the light starts on, it will only turn off if the wrong thing is placed/removed
            // This is safer to implement in the events.
        }

        // Subscribe to the Select events
        socketInteractor.selectEntered.AddListener(OnCrystalSocketed);
        socketInteractor.selectExited.AddListener(OnCrystalRemoved);
    }

    private void OnCrystalSocketed(SelectEnterEventArgs args)
    {
        // Check if the placed object is the REQUIRED crystal
        if (args.interactableObject.transform.GetComponent<XRGrabInteractable>() == requiredCrystal)
        {
            // Correct crystal placed: Turn the light ON
            controlledLight.enabled = true;
            Debug.Log($"Correct crystal ({requiredCrystal.name}) placed in {gameObject.name}. Light activated.", this);
        }
        else if (controlledLight.enabled && lightStartsOff)
        {
            // Wrong crystal placed (and light starts off): Turn the light OFF again if it was somehow on
            controlledLight.enabled = false;
            Debug.Log($"Wrong crystal placed in {gameObject.name}. Light remains OFF.", this);
        }
    }

    private void OnCrystalRemoved(SelectExitEventArgs args)
    {
        // When *any* object is removed, turn the light OFF if it was previously activated by a crystal.
        // This ensures the light is only on when the crystal is physically in the socket.
        controlledLight.enabled = false;
        Debug.Log($"Crystal removed from {gameObject.name}. Light deactivated.", this);
    }

    // Recommended for clean-up
    void OnDestroy()
    {
        if (socketInteractor != null)
        {
            socketInteractor.selectEntered.RemoveListener(OnCrystalSocketed);
            socketInteractor.selectExited.RemoveListener(OnCrystalRemoved);
        }
    }
}