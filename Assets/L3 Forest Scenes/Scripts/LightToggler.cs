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

    [Header("Audio Settings")]
    [Tooltip("Audio Source that will play when trigger is activated.")]
    public AudioSource triggerAudio;

    private Collider triggerCollider;

    void Start()
    {
        triggerCollider = GetComponent<Collider>();

        if (triggerCollider == null)
        {
            Debug.LogError("TimedLightTrigger requires a Collider component on the same GameObject to function!");
            return;
        }

        if (!triggerCollider.isTrigger)
        {
            Debug.LogWarning("The collider on " + gameObject.name + " is not set to 'Is Trigger'. Setting it automatically.");
            triggerCollider.isTrigger = true;
        }

        if (lightsToToggle.Count == 0)
        {
            Debug.LogWarning("TimedLightTrigger on " + gameObject.name + " has no lights assigned.");
        }

        if (triggerAudio == null)
        {
            Debug.LogWarning("No AudioSource assigned to TimedLightTrigger on " + gameObject.name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 🔊 Play audio on trigger enter
        if (triggerAudio != null)
        {
            triggerAudio.Play();
        }

        // Disable the trigger collider so it cannot activate twice
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
            Debug.Log("Trigger disabled. Starting light sequence.");
        }

        // Start the light sequence
        StartCoroutine(LightSequence());
    }

    private IEnumerator LightSequence()
    {
        SetLightsState(true);

        yield return new WaitForSeconds(lightDuration);

        SetLightsState(false);
        Debug.Log("Light sequence complete. Lights OFF.");
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
