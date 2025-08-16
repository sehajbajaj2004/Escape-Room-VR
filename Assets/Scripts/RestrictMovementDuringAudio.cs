using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class RestrictMovementDuringAudio : MonoBehaviour
{
    [Header("Intro Audio Source")]
    public AudioSource introAudio; // Drag & drop your intro AudioSource here

    private DynamicMoveProvider moveProvider;

    private void Start()
    {
        // Auto-find DynamicMoveProvider in the scene
        moveProvider = FindObjectOfType<DynamicMoveProvider>();

        if (introAudio != null && moveProvider != null)
        {
            moveProvider.enabled = false; // Disable movement
            StartCoroutine(ReEnableMovementAfterAudio());
        }
        else
        {
            Debug.LogError("Intro Audio or DynamicMoveProvider not found!");
        }
    }

    private System.Collections.IEnumerator ReEnableMovementAfterAudio()
    {
        if (introAudio.clip != null)
        {
            // Wait for the duration of the clip (safer than isPlaying)
            yield return new WaitForSeconds(introAudio.clip.length);
        }

        moveProvider.enabled = true; // Re-enable movement
    }
}
