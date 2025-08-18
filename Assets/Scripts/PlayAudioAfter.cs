using UnityEngine;
using System.Collections;

public class PlayAudioAfter : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource audioSource; // Assign your AudioSource in the Inspector
    public float delayTime = 3.0f;   // Delay before playing

    private void Start()
    {
        // Start the coroutine when the game starts
        StartCoroutine(PlayAudioAfterDelay());
    }

    private IEnumerator PlayAudioAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayTime);

        // Play the audio
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource not assigned!");
        }
    }
}
