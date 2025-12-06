using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScaleOnGrab : MonoBehaviour
{
    [Header("Target Scale After Grab")]
    public Vector3 scaledSize = new Vector3(0.5f, 0.5f, 0.5f); // Change in inspector

    private XRGrabInteractable grabInteractable;
    private bool isScaled = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            Debug.LogError("XRGrabInteractable not found on this object!");
            return;
        }

        // Add event listener
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        // If already scaled once, do nothing
        if (isScaled) return;

        transform.localScale = scaledSize;
        isScaled = true; // Prevent further scaling
    }
}
