using UnityEngine;

public class LeverTrigger : MonoBehaviour
{
    public HingeJoint leverHinge;

    [Header("GameObjects to Disable")]
    public GameObject objectToDisable1;
    public GameObject objectToDisable2;

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

    private bool hasTriggered = false;

    void Update()
    {
        if (!hasTriggered && leverHinge != null)
        {
            float angle = leverHinge.angle;

            if (angle <= -100f)
            {
                hasTriggered = true;

                // Disable objects
                if (objectToDisable1 != null)
                    objectToDisable1.SetActive(false);

                if (objectToDisable2 != null)
                    objectToDisable2.SetActive(false);

                // Enable object
                if (objectToEnable != null)
                    objectToEnable.SetActive(true);

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
        }
    }
}
