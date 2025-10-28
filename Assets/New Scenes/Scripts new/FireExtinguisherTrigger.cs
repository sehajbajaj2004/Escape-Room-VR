using UnityEngine;

public class FireExtinguishTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fire"))
        {
            ParticleSystem firePS = other.GetComponent<ParticleSystem>();
            if (firePS != null)
            {
                firePS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                Debug.Log("🔥 Fire extinguished: " + other.name);
            }
        }
    }
}
