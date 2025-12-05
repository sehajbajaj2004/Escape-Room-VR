using UnityEngine;

/// <summary>
/// Handles player interaction with a collectible object, including proximity
/// check, sound, and notifying the manager upon successful collection.
/// </summary>
// Ensures the required AudioSource component is automatically added to the GameObject
[RequireComponent(typeof(AudioSource))]
public class CollectibleItem : MonoBehaviour
{
    [Header("Collection Settings")]
    [Tooltip("The maximum distance the player can be from the item to collect it.")]
    public float interactionRange = 3f;

    [Header("Effects")]
    [Tooltip("The sound clip to play upon collection.")]
    public AudioClip collectionSound;

    // Reference to the AudioSource component
    private AudioSource audioSource;
    private GameObject player;

    void Start()
    {
        // Get the AudioSource component attached via RequireComponent
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f; // Make it a 3D sound

        // Find the player/camera object (assumes it is tagged "MainCamera")
        player = GameObject.FindGameObjectWithTag("MainCamera");

        if (player == null)
        {
            Debug.LogError("CollectibleItem: Could not find GameObject with tag 'MainCamera'. " +
                           "Please ensure your player's viewing camera has this tag for proximity checks to work.");
        }
    }

    /// <summary>
    /// Unity function called when the user presses the mouse button while over the collider.
    /// NOTE: This requires the GameObject to have a Collider component (which a Plane does).
    /// </summary>
    private void OnMouseDown()
    {
        // 1. Check if the player is set up
        if (player == null)
        {
            Debug.LogError("Collection failed: Player reference is missing.");
            return;
        }

        // 2. Proximity Check: Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= interactionRange)
        {
            Collect();
        }
        else
        {
            Debug.Log($"Too far away to collect! Distance: {distanceToPlayer}. Required: {interactionRange}.");
            // Optionally play a feedback sound or display a message here
        }
    }

    /// <summary>
    /// Executes the collection sequence.
    /// </summary>
    private void Collect()
    {
        Debug.Log("Item collected successfully.");

        // 1. Play Sound Effect
        if (collectionSound != null)
        {
            // Play the sound once
            audioSource.PlayOneShot(collectionSound);
        }

        // 2. Notify the Manager
        // This invokes the static event, which the Manager is subscribed to.
        if (CollectionManager.OnItemCollected != null)
        {
            CollectionManager.OnItemCollected.Invoke();
        }
        else
        {
            Debug.LogError("CollectionManager.OnItemCollected event is null. Is the CollectionManager script active in the scene?");
        }

        // 3. Disable the object
        // Use Invoke to delay the disabling until after the sound has finished playing
        float delay = (collectionSound != null) ? collectionSound.length : 0f;
        Invoke(nameof(DisableObject), delay);

        // Disable the collider immediately so it can't be clicked again
        GetComponent<Collider>().enabled = false;
    }

    /// <summary>
    /// Disables the GameObject.
    /// </summary>
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}