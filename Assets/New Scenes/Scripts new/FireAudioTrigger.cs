using UnityEngine;

public class FireAudioTrigger : MonoBehaviour
{
    public ParticleSystem fireParticleSystem;
    public AudioSource fireAudioSource;

    private bool wasPlaying = false;

    void Update()
    {
        if (fireParticleSystem == null || fireAudioSource == null)
            return;

        if (fireParticleSystem.isPlaying)
        {
            // Fire just started
            if (!wasPlaying)
            {
                fireAudioSource.loop = true;
                fireAudioSource.Play();
                wasPlaying = true;
            }
        }
        else
        {
            // Fire just stopped
            if (wasPlaying)
            {
                fireAudioSource.Stop();
                wasPlaying = false;
            }
        }
    }
}
