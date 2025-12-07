using UnityEngine;

public class FireAudioManager : MonoBehaviour
{
    [Header("Fire Particle Systems")]
    public ParticleSystem[] fireParticles;   // All fire particle systems

    [Header("Primary Fire Audio Source")]
    public AudioSource fireAudio;           // Main fire loop audio

    [Header("Secondary Fire Audio Source")]
    public AudioSource secondaryFireAudio;  // Additional fire audio

    private bool audioPlaying = false;

    void Update()
    {
        if (fireParticles.Length == 0)
            return;

        bool anyFirePlaying = false;

        // Check if any fire particle system is still active
        foreach (ParticleSystem fire in fireParticles)
        {
            if (fire != null && fire.isPlaying)
            {
                anyFirePlaying = true;
                break;
            }
        }

        // If any fire is burning → ensure both audios are playing
        if (anyFirePlaying && !audioPlaying)
        {
            if (fireAudio != null && !fireAudio.isPlaying)
                fireAudio.Play();

            if (secondaryFireAudio != null && !secondaryFireAudio.isPlaying)
                secondaryFireAudio.Play();

            audioPlaying = true;
        }

        // If all fires are extinguished → stop both audios
        if (!anyFirePlaying && audioPlaying)
        {
            if (fireAudio != null)
                fireAudio.Stop();

            if (secondaryFireAudio != null)
                secondaryFireAudio.Stop();

            audioPlaying = false;
        }
    }
}
