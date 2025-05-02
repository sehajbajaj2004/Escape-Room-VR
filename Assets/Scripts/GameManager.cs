using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerLives = 3;
    private float timeRemaining = 900f; // 15 minutes = 900 seconds

    private Text timerText;         // Reference to Timer Text
    private Canvas playerLossCanvas; // Reference to PlayerLoss Canvas

    void Awake()
    {
        // Singleton Pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindUIElements(); // Manually call for initial scene    
    }

    void Update()
    {
        UpdateTimer();
    }

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timeRemaining / 60);
                int seconds = Mathf.FloorToInt(timeRemaining % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
                Debug.Log("Time: " + timeRemaining);
            }
        }
    }

    public void LoseLife()
    {
        playerLives--;
        if (playerLossCanvas != null)
        {
            playerLossCanvas.enabled = true;
            Invoke(nameof(HidePlayerLossCanvas), 2f); // Hide after 2 seconds
        }

        // Add logic here for what to do if lives reach 0 (optional)
    }

    private void HidePlayerLossCanvas()
    {
        if (playerLossCanvas != null)
            playerLossCanvas.enabled = false;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Called when any scene is loaded
        FindUIElements();
    }

    private void FindUIElements()
    {
        // Find the timer text inside the TimeCanvas
        GameObject timeCanvas = GameObject.Find("TimeCanvas");
        if (timeCanvas != null)
        {
            timerText = timeCanvas.GetComponentInChildren<Text>();
        }

        // Find PlayerLoss canvas
        GameObject lossCanvasObj = GameObject.Find("PlayerLoss");
        if (lossCanvasObj != null)
        {
            playerLossCanvas = lossCanvasObj.GetComponent<Canvas>();
            playerLossCanvas.enabled = false;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
