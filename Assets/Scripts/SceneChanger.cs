using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private AudioSource audioSource; // Drag & drop in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Subscribe to event so audio plays after scene loads
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Unsubscribe after playing
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
