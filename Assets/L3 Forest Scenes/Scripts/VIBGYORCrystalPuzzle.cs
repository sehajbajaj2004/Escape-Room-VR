using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VibgyorCrystalPuzzle : MonoBehaviour
{
    [Header("Socket Interactors (VIBGYOR Order)")]
    public XRSocketInteractor violetSocket;
    public XRSocketInteractor indigoSocket;
    public XRSocketInteractor blueSocket;
    public XRSocketInteractor greenSocket;
    public XRSocketInteractor yellowSocket;
    public XRSocketInteractor orangeSocket;
    public XRSocketInteractor redSocket;

    [Header("Pathway Animation")]
    public Animator pathwayAnimator;
    public string openTrigger = "Open";   // Name of the trigger in Animator

    [Header("Audio")]
    public AudioSource pathwayAudio;

    private bool puzzleCompleted = false;

    void Update()
    {
        if (puzzleCompleted) return;

        // Check each socket for correct crystal tag
        if (IsCorrectCrystal(violetSocket, "VioletCrystal") &&
            IsCorrectCrystal(indigoSocket, "IndigoCrystal") &&
            IsCorrectCrystal(blueSocket, "BlueCrystal") &&
            IsCorrectCrystal(greenSocket, "GreenCrystal") &&
            IsCorrectCrystal(yellowSocket, "YellowCrystal") &&
            IsCorrectCrystal(orangeSocket, "OrangeCrystal") &&
            IsCorrectCrystal(redSocket, "RedCrystal"))
        {
            puzzleCompleted = true;
            TriggerPathway();
        }
    }

    private bool IsCorrectCrystal(XRSocketInteractor socket, string requiredTag)
    {
        if (socket == null) return false;

        if (socket.selectTarget != null)
            return socket.selectTarget.CompareTag(requiredTag);

        return false;
    }

    private void TriggerPathway()
    {
        Debug.Log("🌈 All VIBGYOR Crystals placed correctly! Opening pathway...");

        // Play animation
        if (pathwayAnimator != null)
            pathwayAnimator.SetTrigger(openTrigger);

        // Play audio
        if (pathwayAudio != null)
            pathwayAudio.Play();
    }
}
