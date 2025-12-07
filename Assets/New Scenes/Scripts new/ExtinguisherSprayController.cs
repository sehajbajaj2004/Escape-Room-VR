using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections.Generic;

public class ExtinguisherSprayController : MonoBehaviour
{
    [Header("Particle Settings")]
    public ParticleSystem steamParticles;
    public GameObject steamPrefab;

    [Header("Input Actions")]
    public InputActionReference leftTriggerAction;
    public InputActionReference rightTriggerAction;

    [Header("Audio")]
    public AudioSource steamAudioSource;

    [Header("Grab Settings")]
    public XRGrabInteractable grabInteractable;

    [Header("Wood Object (Assign in Inspector)")]
    public GameObject woodObject;

    [Header("UI to Enable When Grabbed")]
    public GameObject uiObject;     // <-- NEW UI object

    private HashSet<GameObject> extinguishedFires = new HashSet<GameObject>();
    private int fireCount = 0;
    private bool isGrabbed = false;

    private void OnEnable()
    {
        if (leftTriggerAction != null) leftTriggerAction.action.Enable();
        if (rightTriggerAction != null) rightTriggerAction.action.Enable();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
    }

    private void OnDisable()
    {
        if (leftTriggerAction != null) leftTriggerAction.action.Disable();
        if (rightTriggerAction != null) rightTriggerAction.action.Disable();

        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }

    private void Start()
    {
        if (uiObject != null)
            uiObject.SetActive(false);   // UI should be OFF initially
    }

    private void Update()
    {
        if (steamParticles == null || grabInteractable == null)
            return;

        float leftTrigger = leftTriggerAction != null ? leftTriggerAction.action.ReadValue<float>() : 0f;
        float rightTrigger = rightTriggerAction != null ? rightTriggerAction.action.ReadValue<float>() : 0f;

        if (isGrabbed && (leftTrigger > 0.1f || rightTrigger > 0.1f))
        {
            if (!steamParticles.isPlaying)
            {
                steamParticles.Play();
                if (steamAudioSource != null && !steamAudioSource.isPlaying)
                    steamAudioSource.Play();
            }
        }
        else
        {
            if (steamParticles.isPlaying)
            {
                steamParticles.Stop();
                if (steamAudioSource != null && steamAudioSource.isPlaying)
                    steamAudioSource.Stop();
            }
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;

        // Enable UI when picked up
        if (uiObject != null)
            uiObject.SetActive(true);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;

        // Disable UI when released
        if (uiObject != null)
            uiObject.SetActive(false);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag("WildFire") || extinguishedFires.Contains(other))
            return;

        ParticleSystem wildFire = other.GetComponent<ParticleSystem>();
        if (wildFire != null && wildFire.isPlaying)
        {
            extinguishedFires.Add(other);
            wildFire.Stop();

            Debug.Log($"{other.name} extinguished!");

            if (woodObject != null)
                woodObject.SetActive(false);

            if (steamPrefab != null)
            {
                GameObject steam = Instantiate(steamPrefab, other.transform.position, other.transform.rotation);
                steam.transform.localScale = other.transform.localScale;

                ParticleSystem steamPS = steam.GetComponent<ParticleSystem>();
                if (steamPS != null)
                {
                    float totalDuration = steamPS.main.duration + steamPS.main.startLifetime.constantMax;
                    Destroy(steam, totalDuration);
                }
            }

            fireCount++;

            if (fireCount == 10)
            {
                Debug.Log("✅ All 10 fires extinguished!");
            }
        }
    }
}
