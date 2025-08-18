using UnityEngine;

public class ButtonAnimationTrigger : MonoBehaviour
{
    [Header("Animators to Control")]
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;

    [Header("Trigger Parameter Name")]
    [SerializeField] private string triggerName = "Open"; // default to "Open"

    [Header("Audio")]
    [SerializeField] private AudioSource buttonAudio;

    /// <summary>
    /// Call this from XRPushButton OnPress()
    /// </summary>
    public void TriggerAnimations()
    {
        if (animator1 != null)
            animator1.SetTrigger(triggerName);

        if (animator2 != null)
            animator2.SetTrigger(triggerName);

        if (buttonAudio != null)
            buttonAudio.Play();
    }
}
