using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScrewInteraction : MonoBehaviour
{
    private static int screwCount = 0; // Shared among all screws
    public GameObject floorGrill; // Assign this in the inspector
    public string screwTag = "Screw"; // Tag your screw objects
    public XRGrabInteractable grillInteractable; // Assign in inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(screwTag))
        {
            Animation screwAnim = other.GetComponent<Animation>();
            ScrewData screwData = other.GetComponent<ScrewData>();

            if (screwAnim != null && screwData != null && !screwAnim.isPlaying)
            {
                string animName = screwData.animationName;
                if (screwAnim[animName] != null)
                {
                    screwAnim.Play(animName);
                    StartCoroutine(RemoveScrewAfterAnim(screwAnim, animName, other.gameObject));
                }
                else
                {
                    Debug.LogWarning("Animation " + animName + " not found on " + other.name);
                }
            }
        }
    }

    private System.Collections.IEnumerator RemoveScrewAfterAnim(Animation anim, string animName, GameObject screw)
    {
        yield return new WaitForSeconds(anim[animName].length);

        screw.SetActive(false);
        screwCount++;

        if (screwCount >= 4 && grillInteractable != null)
        {
            grillInteractable.enabled = true;
        }
    }
}
