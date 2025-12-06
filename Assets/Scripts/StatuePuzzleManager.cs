using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StatuePuzzleManager : MonoBehaviour
{
    [Header("Socket Interactors")]
    public XRSocketInteractor fireSocket;
    public XRSocketInteractor waterSocket;
    public XRSocketInteractor lightningSocket;  // NEW socket

    [Header("Objects to Enable On Success")]
    public GameObject light1;
    public GameObject light2;

    private bool puzzleCompleted = false;

    private void Start()
    {
        // Make sure lights are OFF initially
        if (light1) light1.SetActive(false);
        if (light2) light2.SetActive(false);
    }

    private void Update()
    {
        if (puzzleCompleted) return;

        // Check all 3 statues
        if (IsCorrectStatue(fireSocket, "FireStatue") &&
            IsCorrectStatue(waterSocket, "WaterStatue") &&
            IsCorrectStatue(lightningSocket, "LightningStatue"))
        {
            Debug.Log("All statues placed correctly! Puzzle Completed.");

            puzzleCompleted = true;

            // Enable the lights
            if (light1) light1.SetActive(true);
            if (light2) light2.SetActive(true);
        }
    }

    // Helper function to check statue tag inside socket
    private bool IsCorrectStatue(XRSocketInteractor socket, string requiredTag)
    {
        if (socket == null) return false;

        if (socket.selectTarget != null)
        {
            return socket.selectTarget.CompareTag(requiredTag);
        }

        return false;
    }
}
