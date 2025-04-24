using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyLockTrigger : MonoBehaviour
{
    [SerializeField] private GameObject keyVisual;
    [SerializeField] private string lockTag = "Lock";
    private bool isInLock = false;
    private Vector3 initialKeyPosition;

    private void Start()
    {
        if (keyVisual != null)
        {
            initialKeyPosition = keyVisual.transform.localPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(lockTag) && !isInLock)
        {
            XRGrabInteractable keyGrab = GetComponentInParent<XRGrabInteractable>();
            if (keyGrab != null && keyGrab.isSelected)
            {
                DoorLockSystem doorLock = other.GetComponentInParent<DoorLockSystem>();
                if (doorLock != null)
                {
                    isInLock = true;
                    SnapKeyToLock(other.transform);
                    doorLock.UnlockDoor();
                    Debug.Log("Key inserted and door unlock triggered!");
                }
            }
        }
    }

    private void SnapKeyToLock(Transform lockTransform)
    {
        Rigidbody keyRb = GetComponentInParent<Rigidbody>();
        if (keyRb != null)
        {
            keyRb.isKinematic = true;
        }

        Transform keyParent = GetComponentInParent<XRGrabInteractable>().transform;
        keyParent.SetPositionAndRotation(lockTransform.position, lockTransform.rotation);
        
        if (keyVisual != null)
        {
            keyVisual.transform.localPosition = initialKeyPosition;
        }
    }
}