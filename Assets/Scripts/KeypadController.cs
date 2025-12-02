using System.Collections.Generic;
using UnityEngine;

public class KeypadController : MonoBehaviour
{
    [Header("Password / Sequence Settings")]
    [Tooltip("Sequence of digits (e.g. \"2401\").")]
    [SerializeField] private string passSequence = "2401";

    [Tooltip("Required number of presses for each digit in the sequence. Array length must match passSequence length.")]
    [SerializeField] private int[] requiredPressCounts = new int[] { 2, 4, 0, 1 };

    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openTrigger = "Open";

    [Header("Audio Feedback (Optional)")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource successSound;
    [SerializeField] private AudioSource errorSound;

    // runtime state
    private int currentIndex = 0;      // which digit in the sequence we're on
    private int currentPressCount = 0; // how many times current digit was pressed

    private void Start()
    {
        // Basic validation
        if (string.IsNullOrEmpty(passSequence))
            Debug.LogError("KeypadController: passSequence is empty.");

        if (requiredPressCounts == null || requiredPressCounts.Length != passSequence.Length)
            Debug.LogWarning("KeypadController: requiredPressCounts length does not match passSequence length. Resizing/initializing to defaults.");

        // If lengths mismatch, try to resize to safe defaults (1 press per digit)
        if (requiredPressCounts == null || requiredPressCounts.Length != passSequence.Length)
        {
            requiredPressCounts = new int[passSequence.Length];
            for (int i = 0; i < requiredPressCounts.Length; i++) requiredPressCounts[i] = 1;
        }

        // Immediately advance past any zero-press digits at the start
        TryAdvanceZeroPressDigits();
    }

    /// <summary>
    /// Call this from each keypad button (e.g. KeypadButton.OnButtonPressed)
    /// with the digit string (e.g. "2", "4", "0", "1").
    /// </summary>
    public void EnterDigit(string digit)
    {
        if (string.IsNullOrEmpty(digit) || digit.Length != 1)
        {
            Debug.LogWarning("KeypadController: EnterDigit received invalid digit: " + digit);
            return;
        }

        // Play generic button sound if assigned
        if (buttonSound != null) buttonSound.Play();

        // If sequence already complete, ignore until reset (or reset immediately)
        if (currentIndex >= passSequence.Length)
            return;

        // Expected digit at current index
        char expected = passSequence[currentIndex];

        if (digit[0] != expected)
        {
            // Wrong digit — reset and play error sound
            Debug.Log($"KeypadController: Wrong digit '{digit}' pressed. Expected '{expected}'. Resetting sequence.");
            if (errorSound != null) errorSound.Play();
            ResetSequence();
            return;
        }

        // Correct digit pressed for this stage — count it
        currentPressCount++;
        Debug.Log($"KeypadController: Correct digit '{digit}' press #{currentPressCount}/{requiredPressCounts[currentIndex]}");

        // If required presses met for this digit, advance to next digit
        if (currentPressCount >= requiredPressCounts[currentIndex])
        {
            currentIndex++;
            currentPressCount = 0;
            Debug.Log($"KeypadController: Advanced to index {currentIndex}.");

            // Auto-advance through any subsequent digits that require 0 presses
            TryAdvanceZeroPressDigits();
        }

        // If sequence finished, trigger success
        if (currentIndex >= passSequence.Length)
        {
            Debug.Log("KeypadController: Sequence complete — unlocking.");
            if (successSound != null) successSound.Play();
            if (doorAnimator != null) doorAnimator.SetTrigger(openTrigger);
            // Reset so puzzle can be attempted again later
            ResetSequence();
        }
    }

    private void TryAdvanceZeroPressDigits()
    {
        // While next digit requires 0 presses, advance
        while (currentIndex < passSequence.Length && requiredPressCounts[currentIndex] == 0)
        {
            Debug.Log($"KeypadController: Auto-advancing index {currentIndex} (0 presses required).");
            currentIndex++;
        }
    }

    private void ResetSequence()
    {
        currentIndex = 0;
        currentPressCount = 0;
        // Also auto-skip initial zeros after reset
        TryAdvanceZeroPressDigits();
    }
}
