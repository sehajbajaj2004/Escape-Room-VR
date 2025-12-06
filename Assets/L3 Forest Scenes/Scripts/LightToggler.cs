using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedLightTrigger : MonoBehaviour
{
    [Header("Light Configuration")]
    [Tooltip("Drag the Light components you want to toggle here.")]
    public List<Light> lightsToToggle = new List<Light>();

    [Tooltip("Duration in seconds the lights will stay ON before turning OFF.")]
    public float lightDuration = 3.0f; 
    
    // Internal reference to the trigger collider on this object
    private Collider triggerCollider;

    void Start()
    {
        // 1. Get the collider component attached to this GameObject
        triggerCollider = GetComponent<Collider>();
        
        // Safety checks
        if (triggerCollider == null)
        {
            Debug.LogError("TimedLightTrigger requires a Collider component on the same GameObject to function!");
            return;
        }

        // 2. Ensure the collider is set to 'Is Trigger' so it doesn't physically block the player
        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning("The collider on " + gameObject.name + " is not set to 'Is Trigger'. Setting it automatically.");
            triggerCollider.isTrigger = true;
        }

        if (lightsToToggle.Count == 0)
        {
            Debug.LogWarning("TimedLightTrigger on " + gameObject.name + " has no lights assigned. Drag your light sources into the list.");
        }
    }

    /// <summary>
    /// Called when another collider enters this GameObject's trigger collider.
    /// This requires the player/triggering object to have a Rigidbody component.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // Optional: Check if the entering object is the Player (using tag or a specific script)
        // if (other.CompareTag("Player")) 
        // {
            
        // Disable the trigger so the event cannot repeat
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
            Debug.Log("Trigger disabled. Starting light sequence.");
        }

        // Start the timed sequence
        StartCoroutine(LightSequence());
        // }
    }

    /// <summary>
    /// Coroutine to handle the timed activation of the lights.
    /// </summary>
    private IEnumerator LightSequence()
    {
        // 1. Turn lights ON
        SetLightsState(true);
        
        // 2. Wait for the specified duration
        yield return new WaitForSeconds(lightDuration);
        
        // 3. Turn lights OFF
        SetLightsState(false);
        Debug.Log("Light sequence complete. Lights OFF.");
    }

    /// <summary>
    /// Helper method to set the enabled state of all lights in the list.
    /// </summary>
    private void SetLightsState(bool state)
    {
        foreach (Light light in lightsToToggle)
        {
            if (light != null)
            {
                light.enabled = state;
                // You can optionally add sound effects here based on 'state'
            }
        }
    }
}