using UnityEngine;

public class CanvasActivatorOnPlayerCollision : MonoBehaviour
{
    public GameObject canvasToEnable;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
