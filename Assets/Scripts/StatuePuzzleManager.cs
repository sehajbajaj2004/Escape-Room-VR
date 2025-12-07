using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StatuePuzzleManager : MonoBehaviour
{
    [Header("Socket Interactors")]
    public XRSocketInteractor fireSocket;
    public XRSocketInteractor waterSocket;
    public XRSocketInteractor lightningSocket;

    [Header("Objects to Enable On Success")]
    public GameObject light1;
    public GameObject light2;

    [Header("Audio on Puzzle Complete")]
    public AudioSource puzzleCompleteAudio;   // <-- NEW

    [Header("Animation Trigger After Delay")]
    public Animator statueAnimator;           // <-- NEW
    public string animationTrigger = "Reveal"; // <-- NEW
    public float animationDelay = 3f;          // <-- NEW

    private bool puzzleCompleted = false;

    private void Start()
    {
        if (light1) light1.SetActive(false);
        if (light2) light2.SetActive(false);
    }

    private void Update()
    {
        if (puzzleCompleted) return;

        if (IsCorrectStatue(fireSocket, "FireStatue") &&
            IsCorrectStatue(waterSocket, "WaterStatue") &&
            IsCorrectStatue(lightningSocket, "LightningStatue"))
        {
            Debug.Log("All statues placed correctly! Puzzle Completed.");

            puzzleCompleted = true;

            // Enable lights
            if (light1) light1.SetActive(true);
            if (light2) light2.SetActive(true);

            // Play completion audio
            if (puzzleCompleteAudio != null)
                puzzleCompleteAudio.Play();

            // Trigger animation after delay
            if (statueAnimator != null)
                Invoke(nameof(TriggerDelayedAnimation), animationDelay);
        }
    }

    private bool IsCorrectStatue(XRSocketInteractor socket, string requiredTag)
    {
        if (socket == null) return false;

        if (socket.selectTarget != null)
            return socket.selectTarget.CompareTag(requiredTag);

        return false;
    }

    // Called after delay
    private void TriggerDelayedAnimation()
    {
        statueAnimator.SetTrigger(animationTrigger);
        Debug.Log("Delayed animation triggered!");
    }
}
