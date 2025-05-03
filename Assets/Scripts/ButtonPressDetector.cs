using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DoubleDoorButtonController : MonoBehaviour
{
    [Header("Door Animations")]
    public Animator leftDoorAnimator;
    public Animator rightDoorAnimator;
    public string openTriggerName = "Open";
    public string closeTriggerName = "Close";

    [Header("Button Settings")]
    public float pressDepth = 0.02f;
    public float pressDuration = 0.5f;
    public float cooldownTime = 1f;
    public bool isToggle = true; // Toggle or momentary button

    [Header("Sound Effects")]
    public AudioClip pressSound;
    public AudioClip releaseSound;
    [Range(0, 1)] public float soundVolume = 0.8f;

    private AudioSource audioSource;
    private Vector3 initialPosition;
    private bool canInteract = true;
    private bool doorsOpen = false;

    void Start()
    {
        initialPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canInteract || !other.CompareTag("Player")) return;

        if (isToggle)
        {
            ToggleDoors();
        }
        else
        {
            OpenDoors();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isToggle && other.CompareTag("Player"))
        {
            CloseDoors();
        }
    }

    void ToggleDoors()
    {
        if (doorsOpen)
        {
            CloseDoors();
        }
        else
        {
            OpenDoors();
        }
    }

    void OpenDoors()
    {
        if (!canInteract) return;

        StartCoroutine(AnimateButton());
        PlaySound(pressSound);

        leftDoorAnimator.SetTrigger(openTriggerName);
        rightDoorAnimator.SetTrigger(openTriggerName);
        doorsOpen = true;
    }

    void CloseDoors()
    {
        if (!canInteract) return;

        StartCoroutine(AnimateButton());
        PlaySound(releaseSound);

        leftDoorAnimator.SetTrigger(closeTriggerName);
        rightDoorAnimator.SetTrigger(closeTriggerName);
        doorsOpen = false;
    }

    System.Collections.IEnumerator AnimateButton()
    {
        canInteract = false;

        // Press down
        float elapsedTime = 0f;
        Vector3 pressedPosition = initialPosition - new Vector3(0, pressDepth, 0);

        while (elapsedTime < pressDuration / 2)
        {
            transform.localPosition = Vector3.Lerp(
                initialPosition,
                pressedPosition,
                elapsedTime / (pressDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Return up
        elapsedTime = 0f;
        while (elapsedTime < pressDuration / 2)
        {
            transform.localPosition = Vector3.Lerp(
                pressedPosition,
                initialPosition,
                elapsedTime / (pressDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;

        if (!isToggle) yield return new WaitForSeconds(cooldownTime);
        canInteract = true;
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip, soundVolume);
        }
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.Log("Added BoxCollider to " + gameObject.name);
        }
    }
#endif
}