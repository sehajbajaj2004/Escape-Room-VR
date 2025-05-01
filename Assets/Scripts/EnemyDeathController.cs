using UnityEngine;

public class EnemyDeathController : MonoBehaviour
{
    [Header("Ground Floor Guard")]
    public Animation groundFloorGuardAnimation;
    public string groundFloorDeathAnimName = "Death";
    
    [Header("Moving Guards")]
    public Animation movementParentAnimation;
    public Animation characterAnimation;
    public string walkAnimName = "Walk";
    public string deathAnimName = "Death";
    
    private bool isDead = false;
    
    public void HandleDeath()
    {
        if (isDead) return;
        isDead = true;
        
        // Determine which type of enemy this is and handle accordingly
        if (groundFloorGuardAnimation != null)
        {
            // Case 1: Ground floor guard
            groundFloorGuardAnimation.Play(groundFloorDeathAnimName);
        }
        else if (movementParentAnimation != null && characterAnimation != null)
        {
            // Case 2 & 3: Moving guards with parent/child structure
            movementParentAnimation.Stop(walkAnimName);
            characterAnimation.Play(deathAnimName);
        }
        
        // Optional: Disable enemy behavior components
               
        // Disable collider to prevent multiple hits
        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;
    }
}