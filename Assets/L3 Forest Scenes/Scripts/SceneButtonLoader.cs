using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonLoader : MonoBehaviour
{
    // Loads Main Menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Loads Cruise Menu scene
    public void LoadMainMenuCruise()
    {
        SceneManager.LoadScene("Main Menu Cruise");
    }

    // Loads Forest Menu scene
    public void LoadMainMenuForest()
    {
        SceneManager.LoadScene("Main Menu Forest");
    }
}
