using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class RestrictMovementDuringAudio : MonoBehaviour
{
    [Header("Intro Audio Source")]
    public AudioSource introAudio; // Drag & drop your intro AudioSource here

    [Header("GameObject to Enable After Audio")]
    public GameObject objectToEnable; // Assign the GameObject you want to enable after audio

    private DynamicMoveProvider moveProvider;

    private void Start()
    {
        // Auto-find DynamicMoveProvider in the scene
        moveProvider = FindObjectOfType<DynamicMoveProvider>();

        if (introAudio != null && moveProvider != null)
        {
            moveProvider.moveSpeed = 0; // Disable movement
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
            // Wait for the duration of the clip (safer than using isPlaying)
            yield return new WaitForSeconds(introAudio.clip.length);
        }

        // Re-enable movement
        moveProvider.moveSpeed = 3;

        // Enable the GameObject if assigned
        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true);
        }
    }
}
