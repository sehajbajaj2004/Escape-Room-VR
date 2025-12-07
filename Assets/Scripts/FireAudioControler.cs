using UnityEngine;

public class FireAudioManager : MonoBehaviour
{
    [Header("Fire Particle Systems")]
    public ParticleSystem[] fireParticles;   // <-- Array of all fire particle systems

    [Header("Fire Audio Source")]
    public AudioSource fireAudio;           // <-- One shared audio source

    private bool audioPlaying = false;

    void Update()
    {
        if (fireParticles.Length == 0 || fireAudio == null)
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

        // If any fire is burning → ensure audio is playing
        if (anyFirePlaying && !audioPlaying)
        {
            fireAudio.Play();
            audioPlaying = true;
        }

        // If all fires are extinguished → stop the audio
        if (!anyFirePlaying && audioPlaying)
        {
            fireAudio.Stop();
            audioPlaying = false;
        }
    }
}
