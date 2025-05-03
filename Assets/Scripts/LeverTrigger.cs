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

    [Header("Sound GameObjects (with AudioSource)")]
    public GameObject leverPullSoundObject;
    public GameObject mechanismSoundObject;

    private AudioSource leverPullSource;
    private AudioSource mechanismSource;

    private bool hasTriggered = false;
    private bool soundPlayed = false;

    void Start()
    {
        if (leverPullSoundObject != null)
            leverPullSource = leverPullSoundObject.GetComponent<AudioSource>();

        if (mechanismSoundObject != null)
            mechanismSource = mechanismSoundObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!hasTriggered && leverHinge != null)
        {
            float angle = leverHinge.angle;

            if (angle <= -30f && !soundPlayed && leverPullSource != null)
            {
                leverPullSource.Play();
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

        SetActiveState(objectToDisable1, false);
        SetActiveState(objectToDisable2, false);
        SetActiveState(objectToDisable3, false);
        SetActiveState(objectToEnable, true);

        // Play looping mechanism sound
        if (mechanismSource != null)
        {
            mechanismSource.loop = true;
            mechanismSource.Play();
        }

        // Play animations
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

    public void ResetLever()
    {
        hasTriggered = false;
        soundPlayed = false;

        if (mechanismSource != null)
        {
            mechanismSource.Stop();
            mechanismSource.loop = false;
        }
    }
}
