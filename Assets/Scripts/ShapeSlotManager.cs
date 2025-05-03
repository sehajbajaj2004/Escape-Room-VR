using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShapePlacementManager : MonoBehaviour
{
    [Header("Socket References")]
    public XRSocketInteractor triangleSocket;
    public XRSocketInteractor squareSocket;
    public XRSocketInteractor circleSocket;

    [Header("Animation")]
    public Animator boxAnimator;
    public string openTriggerName = "OpenBox";

    [Header("Sound Effects")]
    public AudioClip boxOpenSound;
    public AudioClip shapePlacedSound;
    [Range(0, 1)] public float volume = 0.8f;

    private AudioSource audioSource;
    private bool hasPlayedOpenSound = false;

    private void Start()
    {
        // Set up audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.playOnAwake = false;

        // Subscribe to socket events
        triangleSocket.selectEntered.AddListener(OnShapePlaced);
        squareSocket.selectEntered.AddListener(OnShapePlaced);
        circleSocket.selectEntered.AddListener(OnShapePlaced);
    }

    private void OnShapePlaced(SelectEnterEventArgs args)
    {
        // Play shape placed sound
        if (shapePlacedSound != null)
        {
            audioSource.PlayOneShot(shapePlacedSound, volume * 0.6f); // Slightly quieter for placement
        }

        CheckAllShapesPlaced();
    }

    void CheckAllShapesPlaced()
    {
        bool trianglePlaced = IsCorrectObjectInSocket(triangleSocket, "Triangle");
        bool squarePlaced = IsCorrectObjectInSocket(squareSocket, "Cube");
        bool circlePlaced = IsCorrectObjectInSocket(circleSocket, "Cylinder");

        if (trianglePlaced && squarePlaced && circlePlaced && !hasPlayedOpenSound)
        {
            boxAnimator.SetTrigger(openTriggerName);
            PlayBoxOpenSound();
            hasPlayedOpenSound = true;
        }
        else if (!(trianglePlaced && squarePlaced && circlePlaced))
        {
            hasPlayedOpenSound = false;
        }
    }

    void PlayBoxOpenSound()
    {
        if (boxOpenSound != null)
        {
            audioSource.PlayOneShot(boxOpenSound, volume);
        }
    }

    bool IsCorrectObjectInSocket(XRSocketInteractor socket, string expectedTag)
    {
        if (socket.hasSelection)
        {
            var interactable = socket.GetOldestInteractableSelected();
            if (interactable != null && interactable.transform.CompareTag(expectedTag))
            {
                return true;
            }
        }
        return false;
    }

    // Optional: Reset when shapes are removed
    private void OnDestroy()
    {
        triangleSocket.selectEntered.RemoveListener(OnShapePlaced);
        squareSocket.selectEntered.RemoveListener(OnShapePlaced);
        circleSocket.selectEntered.RemoveListener(OnShapePlaced);
    }
}