using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WireConnectionManager : MonoBehaviour
{
    [Header("XR Sockets")]
    public XRSocketInteractor redSocket;
    public XRSocketInteractor yellowSocket;
    public XRSocketInteractor blueSocket;
    public XRSocketInteractor pinkSocket;

    [Header("Animation and Audio")]
    public Animator targetAnimator; // Assign door, machine, etc.
    public string animationTrigger = "Activate"; // Trigger name in Animator
    public AudioSource successSound;

    private bool redConnected = false;
    private bool yellowConnected = false;
    private bool blueConnected = false;
    private bool pinkConnected = false;

    private bool puzzleCompleted = false;

    private void OnEnable()
    {
        // Subscribe to socket events
        redSocket.selectEntered.AddListener(OnWireInserted);
        yellowSocket.selectEntered.AddListener(OnWireInserted);
        blueSocket.selectEntered.AddListener(OnWireInserted);
        pinkSocket.selectEntered.AddListener(OnWireInserted);

        redSocket.selectExited.AddListener(OnWireRemoved);
        yellowSocket.selectExited.AddListener(OnWireRemoved);
        blueSocket.selectExited.AddListener(OnWireRemoved);
        pinkSocket.selectExited.AddListener(OnWireRemoved);
    }

    private void OnDisable()
    {
        // Unsubscribe when disabled
        redSocket.selectEntered.RemoveListener(OnWireInserted);
        yellowSocket.selectEntered.RemoveListener(OnWireInserted);
        blueSocket.selectEntered.RemoveListener(OnWireInserted);
        pinkSocket.selectEntered.RemoveListener(OnWireInserted);

        redSocket.selectExited.RemoveListener(OnWireRemoved);
        yellowSocket.selectExited.RemoveListener(OnWireRemoved);
        blueSocket.selectExited.RemoveListener(OnWireRemoved);
        pinkSocket.selectExited.RemoveListener(OnWireRemoved);
    }

    private void OnWireInserted(SelectEnterEventArgs args)
    {
        GameObject wire = args.interactableObject.transform.gameObject;

        if (args.interactorObject == redSocket && wire.CompareTag("RedWire"))
            redConnected = true;
        else if (args.interactorObject == yellowSocket && wire.CompareTag("YellowWire"))
            yellowConnected = true;
        else if (args.interactorObject == blueSocket && wire.CompareTag("BlueWire"))
            blueConnected = true;
        else if (args.interactorObject == pinkSocket && wire.CompareTag("PinkWire"))
            pinkConnected = true;

        CheckPuzzleCompletion();
    }

    private void OnWireRemoved(SelectExitEventArgs args)
    {
        GameObject wire = args.interactableObject.transform.gameObject;

        if (args.interactorObject == redSocket && wire.CompareTag("RedWire"))
            redConnected = false;
        else if (args.interactorObject == yellowSocket && wire.CompareTag("YellowWire"))
            yellowConnected = false;
        else if (args.interactorObject == blueSocket && wire.CompareTag("BlueWire"))
            blueConnected = false;
        else if (args.interactorObject == pinkSocket && wire.CompareTag("PinkWire"))
            pinkConnected = false;
    }

    private void CheckPuzzleCompletion()
    {
        if (puzzleCompleted) return;

        if (redConnected && yellowConnected && blueConnected && pinkConnected)
        {
            puzzleCompleted = true;
            Debug.Log("✅ All wires connected correctly!");

            if (targetAnimator != null)
                targetAnimator.SetTrigger(animationTrigger);

            if (successSound != null)
                successSound.Play();
        }
    }
}
