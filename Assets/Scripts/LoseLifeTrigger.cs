using UnityEngine;
using System.Collections;

public class LoseLifeTrigger : MonoBehaviour
{
    public AudioClip loseLifeSound; // Assign in Inspector
    private AudioSource audioSource;
    private Collider hazardCollider;
    private bool isCooldown = false;

    private void Start()
    {
        hazardCollider = GetComponent<Collider>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCooldown && other.CompareTag("Player") && GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
            Debug.Log("Player entered hazard. Life deducted.");

            if (loseLifeSound != null)
                audioSource.PlayOneShot(loseLifeSound);

            StartCoroutine(DisableTriggerTemporarily());
        }
    }

    private IEnumerator DisableTriggerTemporarily()
    {
        isCooldown = true;
        hazardCollider.isTrigger = false;
        yield return new WaitForSeconds(5f); // Grace period
        hazardCollider.isTrigger = true;
        isCooldown = false;
    }
}
