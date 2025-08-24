using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.XR;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Stats")]
    [SerializeField] private int playerLives = 3;
    [SerializeField] private float timeRemaining = 900f; // 15 minutes (900 sec)

    [Header("UI References")]
    [SerializeField] private Text timerText;
    [SerializeField] private Canvas playerHealthCanvas; 
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private Canvas gameCompleteCanvas;

    [Header("Feedback")]
    [SerializeField] private AudioSource lifeLossAudio;
    [SerializeField] private Volume globalVolume; // Child of GameManager
    private Vignette vignette;
    private Coroutine vignetteRoutine;

    private readonly List<GameObject> heartIcons = new List<GameObject>();

    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Get Vignette from global volume (once only, since it's child of this GO)
        if (globalVolume != null)
            globalVolume.profile.TryGet(out vignette);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        DisableAllCanvases();
        UpdateTimerUI();

        if (vignette != null)
            vignette.active = false; // ensure it's disabled at start
    }

    void Update()
    {
        UpdateTimer();
    }

    #region Timer
    private void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTimerUI();
        }
        else
        {
            if (gameOverCanvas != null && !gameOverCanvas.enabled)
            {
                Debug.Log("Time Up! Game Over!");
                ShowGameOver();
            }
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
    #endregion

    #region Player Lives
    public void LoseLife()
    {
        if (playerLives <= 0) return;

        playerLives--;
        UpdateHearts();

        // Feedback
        if (lifeLossAudio != null) lifeLossAudio.Play();
        TriggerHaptics(0.7f, 2f);
        ShowVignettePulse();

        if (playerLives <= 0)
        {
            Debug.Log("All Lives Lost! Game Over!");
            ShowGameOver();
        }
    }

    private void UpdateHearts()
    {
        for (int i = 0; i < heartIcons.Count; i++)
            heartIcons[i].SetActive(i < playerLives);
    }
    #endregion

    #region Scene Handling
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene Loaded: {scene.name}");

        // Timer UI
        if (timerText == null)
        {
            GameObject timeCanvas = GameObject.Find("TimeCanvas");
            if (timeCanvas != null)
                timerText = timeCanvas.GetComponentInChildren<Text>();
        }

        // Player Health Canvas
        if (playerHealthCanvas == null)
        {
            GameObject healthCanvasObj = GameObject.Find("PlayerHealthCanvas");
            if (healthCanvasObj != null)
                playerHealthCanvas = healthCanvasObj.GetComponent<Canvas>();
        }

        // Game Over Canvas
        if (gameOverCanvas == null)
        {
            GameObject goCanvasObj = GameObject.Find("GameOverCanvas");
            if (goCanvasObj != null)
                gameOverCanvas = goCanvasObj.GetComponent<Canvas>();
        }

        // Game Complete Canvas
        if (gameCompleteCanvas == null)
        {
            GameObject gcCanvasObj = GameObject.Find("GameCompleteCanvas");
            if (gcCanvasObj != null)
                gameCompleteCanvas = gcCanvasObj.GetComponent<Canvas>();
        }

        DisableAllCanvases();
        UpdateTimerUI();
        StartCoroutine(DelayedHeartReassign());
    }

    private IEnumerator DelayedHeartReassign()
    {
        yield return null; // wait 1 frame

        heartIcons.Clear();

        if (playerHealthCanvas != null)
        {
            AddHeart("h1");
            AddHeart("h2");
            AddHeart("h3");

            UpdateHearts();
        }
    }

    private void AddHeart(string name)
    {
        Transform heart = playerHealthCanvas.transform.Find(name);
        if (heart != null) heartIcons.Add(heart.gameObject);
    }
    #endregion

    #region Game States
    private void DisableAllCanvases()
    {
        if (gameOverCanvas != null) gameOverCanvas.enabled = false;
        if (gameCompleteCanvas != null) gameCompleteCanvas.enabled = false;
    }

    private void ShowGameOver()
    {
        DisableAllCanvases();
        if (gameOverCanvas != null)
            gameOverCanvas.enabled = true;
    }

    public void ShowGameComplete()
    {
        DisableAllCanvases();
        if (gameCompleteCanvas != null)
            gameCompleteCanvas.enabled = true;
    }
    #endregion

    #region Visual + Haptics Feedback
    private void ShowVignettePulse()
    {
        if (vignette == null) return;
        if (vignetteRoutine != null) StopCoroutine(vignetteRoutine);
        vignetteRoutine = StartCoroutine(VignetteRoutine());
    }

    private IEnumerator VignetteRoutine()
    {
        vignette.active = true;
        yield return new WaitForSeconds(5f);
        vignette.active = false;
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
    #endregion

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
