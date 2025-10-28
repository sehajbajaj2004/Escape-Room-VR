using UnityEngine;
using System.Collections.Generic;

public class ExtinguishFire : MonoBehaviour
{
    public ParticleSystem fireExtinguisher;
    public GameObject steamPrefab;

    // Track which wild fires we've already extinguished
    private HashSet<GameObject> extinguishedFires = new HashSet<GameObject>();

    void OnParticleCollision(GameObject other)
    {
        // Only act on objects tagged as WildFire
        if (other.CompareTag("WildFire") && !extinguishedFires.Contains(other))
        {
            ParticleSystem wildFire = other.GetComponent<ParticleSystem>();
            if (wildFire != null && wildFire.isPlaying)
            {
                // Stop the wildfire and add it to the set
                wildFire.Stop();
                extinguishedFires.Add(other);
                Debug.Log($"{other.name} extinguished!");

                // Spawn steam once
                if (steamPrefab != null)
                {
                    GameObject steam = Instantiate(
                        steamPrefab,
                        wildFire.transform.position,
                        wildFire.transform.rotation
                    );

                    // Optional: destroy steam after effect finishes
                    ParticleSystem steamPS = steam.GetComponent<ParticleSystem>();
                    if (steamPS != null)
                    {
                        Destroy(steam, steamPS.main.duration + steamPS.main.startLifetime.constantMax);
                    }
                }
            }
        }
    }
}
