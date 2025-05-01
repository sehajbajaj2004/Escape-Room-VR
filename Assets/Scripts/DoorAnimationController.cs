using UnityEngine;

public class DoorAnimatorController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Call this from animation events if needed
    public void OnDoorOpened()
    {
        Debug.Log("Door opened");
    }

    public void OnDoorClosed()
    {
        Debug.Log("Door closed");
    }
}