using UnityEngine;
using UnityEngine.XR; // for haptic feedback

public class SpotlightDetector : MonoBehaviour
{
    [Header("References")]
    public GameObject gameOverPanel;

    [Header("Haptics Settings")]
    public float hapticAmplitude = 0.7f; // vibration strength (0–1)
    public float hapticDuration = 2f;    // vibration duration in seconds

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Trigger haptics immediately
            TriggerHaptics(hapticAmplitude, hapticDuration);

            //if (gameOverPanel != null)
            //    gameOverPanel.SetActive(true);

            //Time.timeScale = 0f; // Freeze the game
            //Debug.Log("💡 Spotlight detected player -> GAME OVER!");
            // Call LoseLife from GameManager
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoseLife();
                Debug.Log("❤️ Player lost a life!");
            }
            else
            {
                Debug.LogError("❌ GameManager instance not found!");
            }
        }
    }

    private void TriggerHaptics(float amplitude, float duration)
    {
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (leftHand.isValid)
            leftHand.SendHapticImpulse(0, amplitude, duration);

        if (rightHand.isValid)
            rightHand.SendHapticImpulse(0, amplitude, duration);
    }
}
