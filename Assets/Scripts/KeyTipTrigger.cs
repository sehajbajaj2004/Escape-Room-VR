using UnityEngine;

public class KeyTipTrigger : MonoBehaviour
{
    public string lockTag = "Lock";
    private bool hasUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasUnlocked && other.CompareTag(lockTag))
        {
            DoorUnlocker doorUnlocker = other.GetComponentInParent<DoorUnlocker>();
            if (doorUnlocker != null)
            {
                doorUnlocker.UnlockDoor();
                hasUnlocked = true;
            }
        }
    }
}
