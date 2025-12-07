using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class LightningSocketEffect : MonoBehaviour
{
    [Header("XR Socket Interactor")]
    public XRSocketInteractor socket;

    [Header("Lightning Particle System")]
    public ParticleSystem lightningVFX;

    [Header("Playback Speed")]
    public float playbackSpeed = 0.15f;

    [Header("Lightning Statue to Enable")]
    public GameObject lightningStatue;   // <-- NEW

    [Header("Sound Effect")]
    public AudioSource lightningSound;   // <-- NEW

    private void Start()
    {
        if (socket != null)
            socket.selectEntered.AddListener(OnObjectSnapped);

        if (lightningVFX != null)
            lightningVFX.Stop();

        if (lightningStatue != null)
            lightningStatue.SetActive(false);
    }

    private void OnDestroy()
    {
        if (socket != null)
            socket.selectEntered.RemoveListener(OnObjectSnapped);
    }

    private void OnObjectSnapped(SelectEnterEventArgs args)
    {
        // Only respond for the correct object
        if (args.interactableObject.transform.CompareTag("LightningBolt"))
        {
            Debug.Log("⚡ Lightning object snapped — preparing FX!");

            StartCoroutine(PlayLightningSequence(args.interactableObject.transform.gameObject));
        }
    }

    private IEnumerator PlayLightningSequence(GameObject snappedObject)
    {
        // Wait 2 seconds before lightning plays
        yield return new WaitForSeconds(2f);

        if (lightningVFX != null)
        {
            var main = lightningVFX.main;
            main.simulationSpeed = playbackSpeed;

            lightningVFX.Play();
        }

        // Play sound effect
        if (lightningSound != null)
            lightningSound.Play();

        // Disable snapped object
        if (snappedObject != null)
            snappedObject.SetActive(false);

        // Disable the socket
        if (socket != null)
            socket.gameObject.SetActive(false);

        // Enable Lightning Statue
        if (lightningStatue != null)
            lightningStatue.SetActive(true);

        Debug.Log("⚡ Lightning FX played, object disabled, statue enabled!");
    }
}
