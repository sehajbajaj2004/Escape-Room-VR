using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbProvider : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    private XRDirectInteractor interactor;
    private bool isClimbing = false;
    private Vector3 previousHandPosition;

    private void Start()
    {
        interactor = GetComponent<XRDirectInteractor>();
    }

    private void Update()
    {
        if (isClimbing)
        {
            Vector3 handDelta = previousHandPosition - interactor.transform.position;
            playerRigidbody.MovePosition(playerRigidbody.position + handDelta);
            previousHandPosition = interactor.transform.position;
        }
    }

    public void StartClimbing()
    {
        isClimbing = true;
        previousHandPosition = interactor.transform.position;
        playerRigidbody.useGravity = false;
    }

    public void StopClimbing()
    {
        isClimbing = false;
        playerRigidbody.useGravity = true;
    }
}
