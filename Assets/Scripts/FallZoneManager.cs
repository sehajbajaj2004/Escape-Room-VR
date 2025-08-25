using UnityEngine;

public class FallZonesManager : MonoBehaviour
{
    [Header("Zone Objects")]
    [SerializeField] private GameObject fallActivator;   // Trigger collider
    [SerializeField] private GameObject fallDetector;    // Trigger collider (start disabled)

    [Header("Player")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private Transform player; // optional; will auto-find on first trigger if null

    [Header("Fall Detection")]
    [Tooltip("Must drop at least this many meters from activation point to count as a fall.")]
    [SerializeField] private float minFallDistance = 2.0f;
    [Tooltip("Downward speed (m/s) to count as falling. Negative = downward.")]
    [SerializeField] private float fallSpeedThreshold = -2.0f;
    [Tooltip("If no fall within this window after activation, detector auto-disarms.")]
    [SerializeField] private float activationTimeout = 5.0f;

    private float startY;
    private float lastY;
    private float verticalSpeed;
    private bool detectorActive;
    private bool armed;                 // true only once we detect real falling
    private float armedUntilTime;       // timeout clock

    void Awake()
    {
        if (fallDetector != null) fallDetector.SetActive(false);
    }

    void Update()
    {
        if (!detectorActive || player == null) return;

        // Track vertical speed
        float currentY = player.position.y;
        verticalSpeed = (currentY - lastY) / Mathf.Max(Time.unscaledDeltaTime, 0.0001f);
        lastY = currentY;

        // Arm if the player actually falls (distance OR speed)
        if (!armed)
        {
            bool droppedEnough = (startY - currentY) >= minFallDistance;
            bool movingDownFast = verticalSpeed <= fallSpeedThreshold;
            if (droppedEnough || movingDownFast)
                armed = true;
        }

        // Auto-disarm if no fall happened within the window
        if (!armed && Time.time >= armedUntilTime)
        {
            DisarmDetector();
        }
    }

    // Called by relay on Activator enter
    public void OnActivatorTriggered(Transform playerRoot)
    {
        if (detectorActive) return;

        // Resolve player
        if (player == null) player = playerRoot;

        // Arm window starts now
        startY = player.position.y;
        lastY = startY;
        armed = false;
        detectorActive = true;
        armedUntilTime = Time.time + activationTimeout;

        if (fallDetector != null) fallDetector.SetActive(true);

        // (Optional) ensure activator is assigned but no other action needed
    }

    // Called by relay on Detector enter
    public void OnDetectorTriggered(Transform playerRoot)
    {
        if (!detectorActive) return;

        // Decide if this counts as a fall
        float drop = startY - playerRoot.position.y;
        bool droppedEnough = drop >= minFallDistance;
        bool movingDownFast = verticalSpeed <= fallSpeedThreshold;
        bool isFall = armed || droppedEnough || movingDownFast;

        DisarmDetector();

        if (isFall)
        {
            // Lose life
            if (GameManager.Instance != null)
                GameManager.Instance.LoseLife();
        }
        // else: player arrived normally -> no penalty
    }

    private void DisarmDetector()
    {
        detectorActive = false;
        armed = false;
        if (fallDetector != null) fallDetector.SetActive(false);
    }

    // Expose for relay setup checks
    public string PlayerTag => playerTag;
}
