using UnityEngine;

public class CanvasActivatorOnPlayerCollision : MonoBehaviour
{
    public GameObject canvasToEnable;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (canvasToEnable != null)
            {
                canvasToEnable.SetActive(true);
            }

            // End the game (pause everything)
            Time.timeScale = 0f;
            Debug.Log("Game Over - Player collided");
        }
    }
}
