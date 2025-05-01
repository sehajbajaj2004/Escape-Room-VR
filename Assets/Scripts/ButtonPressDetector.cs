using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class ButtonPressDetector : MonoBehaviour
{
    [Header("References")]
    public Animator doorAnimator;
    public string animationTriggerName = "OpenDoor";
    public float cooldownTime = 1f;
    public AudioClip pressSound;

    private AudioSource audioSource;
    private bool canPress = true;
    private XRBaseInteractor currentInteractor;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canPress) return;

        // Check if it's a controller or hand
        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInteractor>();
        if (interactor != null && currentInteractor == null)
        {
            currentInteractor = interactor;
            PressButton();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        XRBaseInteractor interactor = other.GetComponentInParent<XRBaseInteractor>();
        if (interactor == currentInteractor)
        {
            currentInteractor = null;
        }
    }

    void PressButton()
    {
        if (!canPress) return;

        // Trigger animation
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(animationTriggerName);
        }

        // Play sound
        if (pressSound != null)
        {
            audioSource.PlayOneShot(pressSound);
        }

        // Visual feedback
        transform.localPosition -= new Vector3(0, 0.01f, 0);

        StartCoroutine(ButtonCooldown());
    }

    System.Collections.IEnumerator ButtonCooldown()
    {
        canPress = false;
        yield return new WaitForSeconds(cooldownTime);
        canPress = true;

        // Reset button position
        transform.localPosition += new Vector3(0, 0.01f, 0);
    }
}