using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Handles VR grab interaction with a plank. The plank is collected when the player successfully 
/// grabs and then releases it (Select Exit event).
/// </summary>
// MANDATORY for VR grab/interactability. The object must have a Collider and a Rigidbody.
[RequireComponent(typeof(XRGrabInteractable))] 
[RequireComponent(typeof(Rigidbody))] 
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Collider))]
public class PlankCollectible : MonoBehaviour
{
    // The interaction range check is no longer needed as the XR Interaction Toolkit handles this internally.
    // The distance is defined by the attached XR Interactor.
    
    [Header("Effects")]
    [Tooltip("The sound clip to play upon collection.")]
    public AudioClip collectionSound;

    // References
    private PlankResource resourceManager;
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;
    private bool isCollected = false;

    void Start()
    {
        // 1. Find the resource manager
        resourceManager = FindObjectOfType<PlankResource>();
        if (resourceManager == null)
        {
            Debug.LogError("PlankCollectible: PlankResource manager not found in the scene! Cannot collect plank.");
            enabled = false;
            return;
        }

        // 2. Get VR and Audio components
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        // 3. Subscribe to the event that fires when the object is released after a successful grab.
        // This is a reliable point to execute collection logic.
        grabInteractable.selectExited.AddListener(OnPlankReleased);

        // 4. Ensure Rigidbody is configured for a collectible item (usually non-kinematic)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = true; // Allow planks to be affected by gravity
            rb.isKinematic = false; // Allow physics forces
        }
    }

    /// <summary>
    /// Called when the player releases the plank after grabbing it.
    /// This is the new trigger for collection.
    /// </summary>
    /// <param name="args">Interaction event arguments.</param>
    private void OnPlankReleased(SelectExitEventArgs args)
    {
        if (isCollected) return;
        
        // The grab has completed, proceed with collection.
        CollectPlank();
    }
    
    /// <summary>
    /// Executes the collection sequence.
    /// </summary>
    private void CollectPlank()
    {
        isCollected = true;
        
        // 1. Notify the Manager to increase the count
        resourceManager.AddPlank();
        
        // 2. Play Sound Effect
        if (collectionSound != null)
        {
            audioSource.PlayOneShot(collectionSound);
        }
        
        // 3. Disable the VR grab component and collider immediately
        grabInteractable.enabled = false;
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
        
        // 4. Disable the object after the sound finishes playing (to make it disappear)
        float delay = (collectionSound != null) ? collectionSound.length : 0f;
        Invoke(nameof(DisableObject), delay);
        
        Debug.Log("Plank collected successfully via VR grab and release.");
    }
    
    /// <summary>
    /// Disables the GameObject.
    /// </summary>
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
    
    void OnDestroy()
    {
        // Clean up listener when the object is destroyed to prevent memory leaks
        if (grabInteractable != null)
        {
            grabInteractable.selectExited.RemoveListener(OnPlankReleased);
        }
    }
}