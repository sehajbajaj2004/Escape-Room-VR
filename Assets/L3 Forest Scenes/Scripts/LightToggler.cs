using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimedLightTrigger : MonoBehaviour
{
    [Header("Light Configuration")]
    [Tooltip("Drag the Light components you want to toggle here.")]
    public List<Light> lightsToToggle = new List<Light>();

    [Tooltip("Duration in seconds the lights will stay ON before turning OFF.")]
    public float lightDuration = 3.0f;

    [Header("Audio")]
    [Tooltip("Audio that plays when the lights turn ON.")]
    public AudioSource triggerAudio;   // Drag & Drop your audio source here

    // Internal reference to the trigger collider on this object
    private Collider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider>();

        if (triggerCollider == null)
        {
            Debug.LogError("TimedLightTrigger requires a Collider component on the same GameObject!");
            return;
        }

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning("Collider is not set to Trigger. Setting automatically.");
            triggerCollider.isTrigger = true;
        }

        if (lightsToToggle.Count == 0)
        {
            Debug.LogWarning("No lights assigned in TimedLightTrigger.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Disable trigger so this only happens once
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
            Debug.Log("Trigger entered → Lights + Audio activated.");
        }

        // Play the audio immediately
        if (triggerAudio != null)
        {
            triggerAudio.Play();
        }

        // Start timed light sequence
        StartCoroutine(LightSequence());
    }

    private IEnumerator LightSequence()
    {
        SetLightsState(true);

        yield return new WaitForSeconds(lightDuration);

        SetLightsState(false);
        Debug.Log("Light sequence complete.");
    }

    private void SetLightsState(bool state)
    {
        foreach (Light light in lightsToToggle)
        {
            if (light != null)
            {
                light.enabled = state;
            }
        }
    }
}
