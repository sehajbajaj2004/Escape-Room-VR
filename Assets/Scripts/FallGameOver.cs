using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class FallDetectorZoneController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Collider fallTriggerCollider;
    public GameObject gameOverPanel;
    public GameObject vignetteEffect; // Assign your vignette GameObject here (keep disabled initially)

    [Header("Settings")]
    public float enableHeight = 1.5f;
    public float gameOverDelay = 1f;

    private bool hasFallen = false;

    private void Start()
    {
        if (fallTriggerCollider == null)
            fallTriggerCollider = GetComponent<Collider>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (vignetteEffect != null)
            vignetteEffect.SetActive(false);

        fallTriggerCollider.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        bool shouldEnable = player.position.y > enableHeight;
        fallTriggerCollider.enabled = shouldEnable;

        if (!shouldEnable)
            hasFallen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasFallen)
        {
            hasFallen = true;
            Debug.Log("⚠️ FALL DETECTED! Starting GameOver delay...");

            StartCoroutine(GameOverRoutine());
        }
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSeconds(gameOverDelay);

        // Enable vignette + haptic feedback
        if (vignetteEffect != null)
            vignetteEffect.SetActive(true);

        float hapticDuration = 2f;
        TriggerHaptics(0.9f, hapticDuration);

        // Wait for vibration duration before disabling vignette
        yield return new WaitForSeconds(hapticDuration);

        if (vignetteEffect != null)
            vignetteEffect.SetActive(false);

        // Show Game Over UI
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
        Debug.Log("💀 GAME OVER!");
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
