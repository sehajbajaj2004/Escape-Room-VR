using UnityEngine;
using TMPro;

public class KeypadController : MonoBehaviour
{
    [Header("Password Settings")]
    [SerializeField] private string correctPassword = "2401";

    [Header("Display")]
    public TMP_Text displayText;

    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string openTrigger = "Open";

    [Header("Audio (Optional)")]
    [SerializeField] private AudioSource buttonSound;
    [SerializeField] private AudioSource successSound;
    [SerializeField] private AudioSource errorSound;

    private string currentInput = "";

    private void Start()
    {
        ResetDisplay();
    }

    public void EnterDigit(string digit)
    {
        if (buttonSound != null)
            buttonSound.Play();

        // Accept only 4 digits max
        if (currentInput.Length >= 4)
            return;

        // Add digit and update display
        currentInput += digit;
        displayText.text = currentInput; // <-- NO ZEROES DURING INPUT

        // If 4 digits entered → check password
        if (currentInput.Length == 4)
            ValidatePassword();
    }

    private void ValidatePassword()
    {
        if (currentInput == correctPassword)
        {
            if (successSound != null)
                successSound.Play();

            if (doorAnimator != null)
                doorAnimator.SetTrigger(openTrigger);

            // KEEP DISPLAY AS 2401
        }
        else
        {
            if (errorSound != null)
                errorSound.Play();

            ResetDisplay();
        }
    }

    private void ResetDisplay()
    {
        currentInput = "";
        displayText.text = "0000";  // Only at start or wrong password
    }
}
