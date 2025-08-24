using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet") && GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
            Debug.Log($"Player took damage.");
        }
    }
}