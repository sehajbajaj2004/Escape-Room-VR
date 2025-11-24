using UnityEngine;

public class SpotlightTriggerReveal : MonoBehaviour
{
    [Header("Assign the object to enable/disable")]
    public GameObject objectToReveal;   // Drag the object here (keep disabled initially)

    [Header("Tag of the plane to detect")]
    public string planeTag = "CluePlane"; // Add this tag to your plane

    private int contactCount = 0; // Tracks overlapping

    private void Start()
    {
        if (objectToReveal != null)
            objectToReveal.SetActive(false);  // Ensure disabled at start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(planeTag))
        {
            contactCount++;
            objectToReveal.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(planeTag))
        {
            contactCount--;

            if (contactCount <= 0)
            {
                objectToReveal.SetActive(false);
                contactCount = 0;
            }
        }
    }
}
