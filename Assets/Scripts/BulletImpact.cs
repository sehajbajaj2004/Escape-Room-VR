using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    public float damage = 1f;
    
    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyHit(collision.gameObject);
        }
        
        // Destroy bullet on any collision
        Destroy(gameObject);
    }
    
    private void HandleEnemyHit(GameObject enemy)
    {
        // Try to find the appropriate enemy controller
        EnemyDeathController enemyController = enemy.GetComponentInParent<EnemyDeathController>();
        
        if (enemyController != null)
        {
            enemyController.HandleDeath();
        }
    }
}