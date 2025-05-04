using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerLives = 3;
    private float timeRemaining = 900f; // 15 minutes

    private Text timerText;
    private Canvas playerLossCanvas;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject); // Keeps the same GameManager across scenes
        LoadGameState();
    }



    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        FindUIElements();    
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
            }
        }
    }

    public void LoseLife()
    {
        playerLives--;
        SaveGameState();

        if (playerLossCanvas != null)
        {
            playerLossCanvas.enabled = true;
            Invoke(nameof(HidePlayerLossCanvas), 2f);
        }

        // Optional: Game Over
        if (playerLives <= 0)
        {
            Debug.Log("Game Over");
            // Add Game Over logic here
        }
    }

    private void HidePlayerLossCanvas()
    {
        if (playerLossCanvas != null)
            playerLossCanvas.enabled = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we're in Scene 1, reset the timer to 15 minutes
        if (scene.buildIndex == 0) // or use: if (scene.name == "Scene1")
        {
            timeRemaining = 900f;
            SaveGameState(); // Optional: Save the reset state
        }
        else
        {
            LoadGameState(); // Load previously saved time
        }

        FindUIElements();
    }


    private void FindUIElements()
    {
        GameObject timeCanvas = GameObject.Find("TimeCanvas");
        if (timeCanvas != null)
        {
            timerText = timeCanvas.GetComponentInChildren<Text>();
        }

        GameObject lossCanvasObj = GameObject.Find("PlayerLoss");
        if (lossCanvasObj != null)
        {
            playerLossCanvas = lossCanvasObj.GetComponent<Canvas>();
            playerLossCanvas.enabled = false;
        }
    }

    public void SaveGameState()
    {
        PlayerPrefs.SetInt("PlayerLives", playerLives);
        PlayerPrefs.SetFloat("TimeRemaining", timeRemaining);
        PlayerPrefs.Save();
    }

    public void LoadGameState()
    {
        playerLives = PlayerPrefs.GetInt("PlayerLives", 3);
        timeRemaining = PlayerPrefs.GetFloat("TimeRemaining", 900f);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
