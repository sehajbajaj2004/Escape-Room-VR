using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class SocketSoundController : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip snapSound;
    public AudioClip unsnapSound;
    [Range(0, 1)] public float volume = 0.8f;

    private AudioSource audioSource;
    private XRSocketInteractor socketInteractor;

    void Awake()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f; // 3D sound
        audioSource.playOnAwake = false;
    }

    void OnEnable()
    {
        socketInteractor.selectEntered.AddListener(OnObjectSnapped);
        socketInteractor.selectExited.AddListener(OnObjectUnsnapped);
    }

    void OnDisable()
    {
        socketInteractor.selectEntered.RemoveListener(OnObjectSnapped);
        socketInteractor.selectExited.RemoveListener(OnObjectUnsnapped);
    }

    private void OnObjectSnapped(SelectEnterEventArgs args)
    {
        PlaySnapSound();
    }

    private void OnObjectUnsnapped(SelectExitEventArgs args)
    {
        PlayUnsnapSound();
    }

    public void PlaySnapSound()
    {
        if (snapSound != null)
        {
            audioSource.PlayOneShot(snapSound, volume);
        }
    }

    public void PlayUnsnapSound()
    {
        if (unsnapSound != null)
        {
            audioSource.PlayOneShot(unsnapSound, volume);
        }
    }
}