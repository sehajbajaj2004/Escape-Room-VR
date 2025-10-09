using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeypadButton : MonoBehaviour
{
    public string digit;
    public KeypadController keypadController;

    private void Start()
    {
        // Try to auto-assign controller if not set manually
        if (keypadController == null)
            keypadController = FindObjectOfType<KeypadController>();
    }

    public void OnButtonPressed()
    {
        keypadController.EnterDigit(digit);
    }
}
