using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public HingeJoint leverHinge;

    [Header("GameObjects to Disable")]
    public GameObject objectToDisable1;
    public GameObject objectToDisable2;
    public GameObject objectToDisable3;

    [Header("GameObject to Enable")]
    public GameObject objectToEnable;

    [System.Serializable]
    public class AnimationTarget
    {
        public GameObject targetObject;
        public string animationName;
    }

    [Header("Animations To Play")]
    public AnimationTarget[] animationsToPlay;

    [Header("Sound Effects")]
    public AudioClip leverPullSound;
    public AudioClip mechanismSound;
    [Range(0, 1)] public float soundVolume = 0.8f;

    private AudioSource audioSource;
    private bool hasTriggered = false;
    private bool soundPlayed = false;

    void Start()
    {
        // Set up audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f; // 3D sound
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        if (!hasTriggered && leverHinge != null)
        {
            float angle = leverHinge.angle;

            // Play sound when lever starts moving past threshold
            if (angle <= -30f && !soundPlayed && leverPullSound != null)
            {
                audioSource.PlayOneShot(leverPullSound, soundVolume);
                soundPlayed = true;
            }

            if (angle <= -100f)
            {
                TriggerEffects();
            }
        }
    }

    void TriggerEffects()
    {
        hasTriggered = true;

        // Disable objects
        SetActiveState(objectToDisable1, false);
        SetActiveState(objectToDisable2, false);
        SetActiveState(objectToDisable3, false);

        // Enable object
        SetActiveState(objectToEnable, true);

        // Play mechanism sound
        if (mechanismSound != null)
        {
            audioSource.PlayOneShot(mechanismSound, soundVolume * 0.8f);
        }

        // Play legacy animations
        foreach (var animTarget in animationsToPlay)
        {
            if (animTarget.targetObject != null)
            {
                Animation anim = animTarget.targetObject.GetComponent<Animation>();
                if (anim != null)
                {
                    anim.Play(animTarget.animationName);
                }
                else
                {
                    Debug.LogWarning("Missing Animation component on: " + animTarget.targetObject.name);
                }
            }
        }
    }

    void SetActiveState(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
        }
    }

    // Optional: Reset lever for multiple uses
    public void ResetLever()
    {
        hasTriggered = false;
        soundPlayed = false;
    }
}