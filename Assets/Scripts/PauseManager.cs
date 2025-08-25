using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("XR Origins")]
    public GameObject playerXROrigin;
    public GameObject carXROrigin;
    public GameObject pauseXROrigin;

    [Header("Input")]
    public InputActionReference pauseAction; // Menu button on left controller

    [Header("Audio")]
    public AudioListener playerAudioListener;
    public AudioListener carAudioListener;
    public AudioListener pauseAudioListener;

    private enum OriginState { Player, Car, None }
    private OriginState previousState = OriginState.None;

    private bool isPaused = false;

    private void OnEnable()
    {
        pauseAction.action.performed += OnPausePressed;
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.performed -= OnPausePressed;
        pauseAction.action.Disable();
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (!isPaused)
        {
            PauseGame();
            // Time.timeScale = 0;
        }
        else
        {
            ResumeGame();
            // Time.timeScale = 1;
        }
    }

    private void PauseGame()
    {
        if (playerXROrigin.activeSelf)
        {
            previousState = OriginState.Player;
            playerXROrigin.SetActive(false);
            playerAudioListener.enabled = false;
        }
        else if (carXROrigin.activeSelf)
        {
            previousState = OriginState.Car;
            carXROrigin.SetActive(false);
            carAudioListener.enabled = false;
        }

        pauseXROrigin.SetActive(true);
        pauseAudioListener.enabled = true;

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseXROrigin.SetActive(false);
        pauseAudioListener.enabled = false;

        if (previousState == OriginState.Player)
        {
            playerXROrigin.SetActive(true);
            playerAudioListener.enabled = true;
        }
        else if (previousState == OriginState.Car)
        {
            carXROrigin.SetActive(true);
            carAudioListener.enabled = true;
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Unpause before restarting
        SceneManager.LoadScene("Level 1");
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
