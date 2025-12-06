using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a single broken spot on the bridge that is repaired instantly
/// when the player walks into its trigger collider, provided they have enough planks.
/// </summary>
[RequireComponent(typeof(Collider))] // Mandatory for trigger
[RequireComponent(typeof(AudioSource))]
public class BridgeGap : MonoBehaviour
{
    [Header("Repair Settings")]
    [Tooltip("The number of planks required from the inventory to repair this specific gap.")]
    public int planksRequiredToRepair = 1;
    
    [Tooltip("The plank GameObject that is initially DISABLED and will be enabled upon repair.")]
    public GameObject fixedPlankObject;
    
    [Tooltip("The sound clip to play when a plank is successfully placed.")]
    public AudioClip placementSound;

    // References to the resource manager and local components
    private PlankResource resourceManager;
    private AudioSource audioSource;
    private MeshRenderer meshRenderer; // New reference to hide the broken mesh visualization
    private bool isRepaired = false;

    void Start()
    {
        // 1. Find the resource manager in the scene
        resourceManager = FindObjectOfType<PlankResource>();
        if (resourceManager == null)
        {
            Debug.LogError("BridgeGap: PlankResource manager not found in the scene! Cannot proceed with repair.");
            enabled = false;
            return;
        }

        // 2. Get audio component
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        
        // 3. Ensure the collider is set as a trigger
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning($"BridgeGap collider on {gameObject.name} must be set to 'Is Trigger'. Setting automatically.");
            col.isTrigger = true;
        }
        
        // 4. Get and disable the MeshRenderer of THIS object (the broken gap trigger)
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false; // Makes the gap visually invisible
        }
        
        // 5. Ensure the fixed plank object starts disabled (the visual fix)
        if (fixedPlankObject != null && fixedPlankObject.activeSelf)
        {
            fixedPlankObject.SetActive(false);
        }
    }

    /// <summary>
    /// Called when another collider enters this trigger collider.
    /// This is the new trigger for placing the plank.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the gap is already repaired or if the colliding object is not the player
        // ASSUMPTION: The player's main collider is tagged "Player".
        if (isRepaired || !other.CompareTag("Player")) 
        {
            return;
        }
        
        AttemptRepair();
    }

    /// <summary>
    /// Logic for repairing the gap.
    /// </summary>
    private void AttemptRepair()
    {
        // 1. Check and deduct required resource from the Manager
        if (resourceManager.TryUsePlanks(planksRequiredToRepair))
        {
            // Resource spent successfully!
            isRepaired = true;
            
            // 2. Play Audio
            if (placementSound != null)
            {
                audioSource.PlayOneShot(placementSound);
            }

            // 3. Visual Change (Enable the fixed plank)
            if (fixedPlankObject != null)
            {
                fixedPlankObject.SetActive(true);
            }
            
            // 4. Notify Manager of successful repair
            resourceManager.GapRepaired();
            
            // 5. Disable this trigger so the spot cannot be repaired twice
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
            
            Debug.Log("Bridge gap repaired via collision.");
        }
        else
        {
            Debug.Log("Cannot repair gap via collision: Not enough planks.");
        }
    }
}