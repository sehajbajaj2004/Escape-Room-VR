using UnityEngine;

public class KeypadController : MonoBehaviour
{
    [Header("Password / Sequence Settings")]
    [Tooltip("Sequence of digits (e.g. \"2401\").")]
    [SerializeField] private string passSequence = "2401";

    [Tooltip("Required number of presses for each digit.")]
    [SerializeField] private int[] requiredPressCounts = new int[] { 2, 4, 0, 1 };

    [Header("Indicators")]
    [Tooltip("Red indicators for each digit in order.")]
    public GameObject[] redIndicators;

    [Tooltip("Green indicators for each digit in order.")]
    public GameObject[] greenIndicators;

    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openTrigger = "Open";

    [Header("Audio Feedback (Optional)")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource successSound;
    [SerializeField] private AudioSource errorSound;

    private int currentIndex = 0;
    private int currentPressCount = 0;

    private void Start()
    {
        // Reset indicators at start
        for (int i = 0; i < redIndicators.Length; i++)
        {
            if (redIndicators[i] != null) redIndicators[i].SetActive(true);
            if (greenIndicators[i] != null) greenIndicators[i].SetActive(false);
        }

        TryAdvanceZeroPressDigits(); // auto-skip any 0-press digits
    }

    public void EnterDigit(string digit)
    {
        if (buttonSound != null) buttonSound.Play();

        // If sequence complete, ignore
        if (currentIndex >= passSequence.Length)
            return;

        char expected = passSequence[currentIndex];

        // Wrong digit → reset everything
        if (digit[0] != expected)
        {
            if (errorSound != null) errorSound.Play();
            ResetSequence();
            return;
        }

        // Correct digit pressed
        currentPressCount++;

        // If the required number of presses reached → update indicator
        if (currentPressCount >= requiredPressCounts[currentIndex])
        {
            // Turn Red OFF, Green ON
            if (redIndicators[currentIndex] != null)
                redIndicators[currentIndex].SetActive(false);

            if (greenIndicators[currentIndex] != null)
                greenIndicators[currentIndex].SetActive(true);

            currentIndex++;
            currentPressCount = 0;

            TryAdvanceZeroPressDigits();
        }

        // Entire sequence complete
        if (currentIndex >= passSequence.Length)
        {
            if (successSound != null) successSound.Play();
            if (doorAnimator != null)
                doorAnimator.SetTrigger(openTrigger);

            ResetSequence();
        }
    }

    private void TryAdvanceZeroPressDigits()
    {
        while (currentIndex < passSequence.Length &&
               requiredPressCounts[currentIndex] == 0)
        {
            // Turn indicators ON instantly for zero-press digits
            if (redIndicators[currentIndex] != null)
                redIndicators[currentIndex].SetActive(false);

            if (greenIndicators[currentIndex] != null)
                greenIndicators[currentIndex].SetActive(true);

            currentIndex++;
        }
    }

    private void ResetSequence()
    {
        currentIndex = 0;
        currentPressCount = 0;

        // Reset indicators
        for (int i = 0; i < redIndicators.Length; i++)
        {
            if (redIndicators[i] != null) redIndicators[i].SetActive(true);
            if (greenIndicators[i] != null) greenIndicators[i].SetActive(false);
        }

        TryAdvanceZeroPressDigits(); // handle zero-press digits again
    }
}
