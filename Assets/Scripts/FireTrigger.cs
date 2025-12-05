using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    public ParticleSystem fireParticle;
    public string blazerTag = "FireBlazer";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(blazerTag))
        {
            Debug.Log("Ignite Fire!");
            if (fireParticle != null && !fireParticle.isPlaying)
            {
                fireParticle.Play();
            }
        }
    }
}
