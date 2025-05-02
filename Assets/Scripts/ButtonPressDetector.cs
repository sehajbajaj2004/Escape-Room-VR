using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ButtonPressDetector : MonoBehaviour
{
    [Header("Animation Settings")]
    public Animator doorAnimator;
    public string animationTriggerName = "OpenDoor";
    public float pressDepth = 0.01f; // How far button moves when pressed
    public float pressDuration = 0.5f;
    public float cooldownTime = 1f;

    [Header("Feedback")]
    public AudioClip pressSound;
    public GameObject pressEffect; // Particle effect prefab

    private Vector3 initialPosition;
    private AudioSource audioSource;
    private bool canPress = true;
    private bool isPressed = false;

    void Start()
    {
        initialPosition = transform.localPosition;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // 3D sound
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canPress || isPressed) return;

        // Check if collision is from player's hand
        if (other.CompareTag("Player"))
        {
            PressButton();
        }
    }

    void PressButton()
    {
        isPressed = true;
        canPress = false;

        // Trigger door animation
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(animationTriggerName);
        }

        // Play sound
        if (pressSound != null)
        {
            audioSource.PlayOneShot(pressSound);
        }

        // Visual effect
        if (pressEffect != null)
        {
            Instantiate(pressEffect, transform.position, Quaternion.identity);
        }

        // Button press animation
        StartCoroutine(MoveButton());
        StartCoroutine(ButtonCooldown());
    }

    System.Collections.IEnumerator MoveButton()
    {
        float elapsedTime = 0f;
        Vector3 pressedPosition = initialPosition - new Vector3(0, pressDepth, 0);

        // Press down
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
        isPressed = false;
    }

    System.Collections.IEnumerator ButtonCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canPress = true;
    }
}