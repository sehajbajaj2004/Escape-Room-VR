using UnityEngine;
using UnityEngine.XR;
using System.Collections;

public class FallDetectorZoneController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Collider fallTriggerCollider;
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
            Debug.Log("⚠️ FALL DETECTED! Starting LoseLife sequence...");

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

            // StartCoroutine(FallRoutine());
        }
    }
}
