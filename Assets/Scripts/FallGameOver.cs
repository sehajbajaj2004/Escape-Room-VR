using UnityEngine;
using System.Collections;

public class FallDetectorZoneController : MonoBehaviour
{
    [Header("References")]
    public Transform player; // XR Origin tagged "Player"
    public Collider fallTriggerCollider;
    public GameObject gameOverPanel; // Assign in Inspector (keep disabled initially)

    [Header("Settings")]
    public float enableHeight = 1.5f; // Height above which fall detection is active
    public float gameOverDelay = 1f;  // Delay before showing GameOver panel

    private bool hasFallen = false;

    private void Start()
    {
        if (fallTriggerCollider == null)
            fallTriggerCollider = GetComponent<Collider>();

        // Find player automatically if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        fallTriggerCollider.enabled = false;
    }

    void Update()
    {
        if (player == null) return;

        // Enable collider only if player is above threshold height
        bool shouldEnable = player.position.y > enableHeight;
        fallTriggerCollider.enabled = shouldEnable;

        // Reset fall detection when player is below threshold
        if (!shouldEnable)
        {
            hasFallen = false;
        }
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
        yield return new WaitForSeconds(1f); // wait 1 second

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f; // Freeze game
        Debug.Log("💀 GAME OVER!");
    }
}
