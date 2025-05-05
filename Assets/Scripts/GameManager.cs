using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int playerLives = 3;
    private float timeRemaining = 900f; // 15 minutes

    [Header("UI References")]
    public Text timerText;               // Assign in Inspector (optional)
    public Canvas playerLossCanvas;      // Assign in Inspector (optional)

    private List<GameObject> heartIcons = new List<GameObject>(); // h1, h2, h3

    void Awake()
    {
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

        if (playerLossCanvas != null)
        {
            playerLossCanvas.enabled = false;
        }
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
        if (playerLives > 0)
        {
            playerLives--;

            UpdateHearts();

            if (playerLossCanvas != null)
            {
                playerLossCanvas.enabled = true;
                Invoke(nameof(HidePlayerLossCanvas), 2f);
            }
        }

        // You can add game over logic here if playerLives <= 0
    }

    private void HidePlayerLossCanvas()
    {
        if (playerLossCanvas != null)
            playerLossCanvas.enabled = false;
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartIcons.Count; i++)
        {
            heartIcons[i].SetActive(i < playerLives);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        // Reassign timerText
        if (timerText == null)
        {
            GameObject timeCanvas = GameObject.Find("TimeCanvas");
            if (timeCanvas != null)
            {
                timerText = timeCanvas.GetComponentInChildren<Text>();
                Debug.Log("Timer Text reassigned from TimeCanvas");
            }
            else
            {
                Debug.LogWarning("TimeCanvas not found in scene: " + scene.name);
            }
        }

        // Reassign PlayerLoss canvas
        if (playerLossCanvas == null)
        {
            GameObject lossCanvasObj = GameObject.Find("PlayerLoss");
            if (lossCanvasObj != null)
            {
                playerLossCanvas = lossCanvasObj.GetComponent<Canvas>();
            }
        }

        if (playerLossCanvas != null)
        {
            playerLossCanvas.enabled = false;
        }

        // Update timer display immediately
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Find hearts again in the new scene
        ReassignHearts();
    }

    void ReassignHearts()
    {
        heartIcons.Clear();
        GameObject healthCanvas = GameObject.Find("PlayerHealthCanvas");

        if (healthCanvas != null)
        {
            Transform h1 = healthCanvas.transform.Find("h1");
            Transform h2 = healthCanvas.transform.Find("h2");
            Transform h3 = healthCanvas.transform.Find("h3");

            if (h1 != null) heartIcons.Add(h1.gameObject);
            if (h2 != null) heartIcons.Add(h2.gameObject);
            if (h3 != null) heartIcons.Add(h3.gameObject);

            UpdateHearts();
        }
        else
        {
            Debug.LogWarning("PlayerHealthCanvas not found in scene.");
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
