using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string sceneName;

    // Called when Play button is clicked
    public void PlayGame()
    {
        SceneManager.LoadScene(sceneName); // make sure Level 1 is added to Build Settings
    }

    // Called when Quit button is clicked
    public void QuitGame()
    {
        Debug.Log("Quit Game"); // For testing in Unity Editor
        Application.Quit();     // Works in a built game (not in editor)
    }

    // Called when "Main Menu" button is clicked
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu"); // ensure MainMenu is in Build Settings
    }
}
