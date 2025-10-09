using System.Collections.Generic;
using UnityEngine;

public class KeypadController : MonoBehaviour
{
    [Header("Password Settings")]
    [SerializeField] private string correctPassword = "1002";
    private string enteredPassword = "";

    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;

    [Header("Audio Feedback (Optional)")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource successSound;
    [SerializeField] private AudioSource errorSound;

    public void EnterDigit(string digit)
    {
        if (enteredPassword.Length >= correctPassword.Length)
            return;

        enteredPassword += digit;
        Debug.Log("Entered: " + enteredPassword);

        if (buttonSound != null) buttonSound.Play();

        // Check if password complete
        if (enteredPassword.Length == correctPassword.Length)
        {
            if (enteredPassword == correctPassword)
            {
                Debug.Log("✅ Correct Password!");
                if (successSound != null) successSound.Play();
                doorAnimator.SetTrigger("Open");
            }
            else
            {
                Debug.Log("❌ Wrong Password!");
                if (errorSound != null) errorSound.Play();
            }

            // Reset input after attempt
            enteredPassword = "";
        }
    }
}
