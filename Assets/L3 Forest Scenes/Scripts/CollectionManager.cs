using UnityEngine;
using System.Collections; // Needed for Coroutines
using System;

/// <summary>
/// Manages the collection of items, tracks the count, and executes a
/// timed light and audio sequence when the target number of items (4) is reached.
/// </summary>
[RequireComponent(typeof(AudioSource))] // Automatically adds AudioSource for playing sound
public class CollectionManager : MonoBehaviour
{
    // The total number of items the player must collect to trigger the event.
    private const int TOTAL_ITEMS_TO_COLLECT = 4;

    // A static C# Action that CollectibleItem scripts will invoke when collected.
    // This allows the CollectibleItem script to notify this manager without needing a direct reference.
    public static Action OnItemCollected;

    [Header("Tracking")]
    [Tooltip("The current number of items collected.")]
    private int itemsCollected = 0;
    private bool triggerActivated = false;

    [Header("Final Light & Audio Trigger")]
    [Tooltip("The first light source to turn on.")]
    public Light lightSource1;
    [Tooltip("The second light source to turn on.")]
    public Light lightSource2;
    [Tooltip("Duration in seconds the lights will stay ON before turning OFF.")]
    public float lightDuration = 5.0f;
    [Tooltip("The audio clip to play when the sequence begins (e.g., a victory chime or dialogue).")]
    public AudioClip sequenceAudioClip;

    private AudioSource audioSource;

    void Awake()
    {
        // Get the AudioSource component that was added by RequireComponent
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // Ensure sound is 3D
    }

    private void OnEnable()
    {
        // Subscribe to the collection event when the manager is enabled.
        OnItemCollected += ItemCollected;
    }

    private void OnDisable()
    {
        // Unsubscribe from the event when the manager is disabled to prevent errors.
        OnItemCollected -= ItemCollected;
    }

    /// <summary>
    /// Called by a CollectibleItem when it is successfully clicked/collected.
    /// </summary>
    private void ItemCollected()
    {
        if (triggerActivated) return;

        itemsCollected++;
        Debug.Log($"Item collected! Current count: {itemsCollected} / {TOTAL_ITEMS_TO_COLLECT}");

        // Check for the collection goal
        if (itemsCollected >= TOTAL_ITEMS_TO_COLLECT)
        {
            triggerActivated = true;
            Debug.Log("COLLECTION GOAL REACHED! Starting light sequence.");
            
            // Start the sequence to play audio and toggle lights
            StartCoroutine(LightAudioSequence());
        }
    }

    /// <summary>
    /// Coroutine to handle the timed activation of the lights and audio.
    /// </summary>
    private IEnumerator LightAudioSequence()
    {
        // 1. Play Audio
        if (sequenceAudioClip != null)
        {
            audioSource.PlayOneShot(sequenceAudioClip);
        }
        
        // 2. Turn Lights ON
        SetLightsState(true);
        
        // 3. Wait for the specified duration
        yield return new WaitForSeconds(lightDuration);
        
        // 4. Turn lights OFF
        SetLightsState(false);
        Debug.Log("Timed sequence complete. Lights OFF.");
    }

    /// <summary>
    /// Helper method to set the enabled state of the two assigned lights.
    /// </summary>
    private void SetLightsState(bool state)
    {
        if (lightSource1 != null)
        {
            lightSource1.enabled = state;
        }
        if (lightSource2 != null)
        {
            lightSource2.enabled = state;
        }

        if (lightSource1 == null && lightSource2 == null)
        {
            Debug.LogWarning("Both light sources are missing on the CollectionManager.");
        }
    }
}