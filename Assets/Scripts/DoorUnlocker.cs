using UnityEngine;

public class DoorUnlocker : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void UnlockDoor()
    {
        // Unfreeze position constraints
        rb.constraints &= ~RigidbodyConstraints.FreezePositionX;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        rb.constraints &= ~RigidbodyConstraints.FreezePositionZ;

        Debug.Log("Door Unlocked!");
    }
}
