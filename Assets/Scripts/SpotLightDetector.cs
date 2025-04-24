using UnityEngine;

public class SpotlightDetector : MonoBehaviour
{
    public GameObject gameOverPanel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0f; // Optional: Freeze the game
            // You can also trigger sound, animation, or restart options here
        }
    }
}
