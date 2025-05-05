using UnityEngine;

public class CanvasTimer : MonoBehaviour
{
    public GameObject canvasToShow;      // Assign in the Inspector
    public float displayTime = 15f;
    public AudioClip introAudioClip;     // Assign your audio clip in Inspector

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        StartCoroutine(PlayAudioThenShowCanvas());
    }

    private System.Collections.IEnumerator PlayAudioThenShowCanvas()
    {
        if (introAudioClip != null)
        {
            audioSource.clip = introAudioClip;
            audioSource.Play();
            yield return new WaitForSeconds(31f);
        }

        canvasToShow.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        canvasToShow.SetActive(false);
    }
}
