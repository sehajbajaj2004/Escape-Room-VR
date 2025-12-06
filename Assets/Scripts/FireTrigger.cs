using UnityEngine;
using System.Collections;

public class FireTrigger : MonoBehaviour
{
    [Header("Fire Settings")]
    public ParticleSystem fireParticle;
    public string blazerTag = "FireBlazer";

    [Header("Object To Activate")]
    public GameObject objectToActivate;   // Drag the object you want to enable
    public float delayBeforeActivation = 3f;  // Time delay before enabling the object

    private bool hasActivated = false;

    private void Start()
    {
        // Ensure the object is disabled at the start
        if (objectToActivate != null)
            objectToActivate.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(blazerTag))
        {
            Debug.Log("Fire blazer entered! Igniting fire...");

            // Start fire particle if not already playing
            if (fireParticle != null && !fireParticle.isPlaying)
                fireParticle.Play();

            // Prevent multiple triggers
            if (!hasActivated)
            {
                hasActivated = true;
                StartCoroutine(ActivateObjectAfterDelay());
            }
        }
    }

    private IEnumerator ActivateObjectAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeActivation);

        if (objectToActivate != null)
        {
            Debug.Log("Activating object after delay!");
            objectToActivate.SetActive(true);
        }
    }
}
