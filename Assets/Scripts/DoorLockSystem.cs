using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(Rigidbody))]
public class DoorLockSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private XRGrabInteractable doorHandle;
    [SerializeField] private Collider lockCollider;
    
    private HingeJoint hinge;
    private Rigidbody doorRigidbody;
    private bool isUnlocked = false;
    
    private void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        doorRigidbody = GetComponent<Rigidbody>();
        LockDoor();
    }
    
    public void UnlockDoor()
    {
        if (isUnlocked) return;
        
        isUnlocked = true;
        
        // Enable physics
        doorRigidbody.isKinematic = false;
        
        // Configure hinge joint
        hinge.useMotor = true;
        hinge.useLimits = true;
        
        // Enable door handle interaction
        doorHandle.enabled = true;
        
        // Optional: Add unlock effects
        // AudioSource.PlayClipAtPoint(unlockSound, transform.position);
        // unlockParticles.Play();
    }
    
    private void LockDoor()
    {
        isUnlocked = false;
        doorRigidbody.isKinematic = true;
        hinge.useMotor = false;
        doorRigidbody.velocity = Vector3.zero;
        doorRigidbody.angularVelocity = Vector3.zero;
        doorHandle.enabled = false;
    }
}