using UnityEngine;
using UnityEngine.Events; // ADDED: This line fixes the CS0246 error

/// <summary>
/// Manages the total number of planks the player has collected and can use
/// to repair the bridge. Also triggers an event when the bridge is complete.
/// </summary>
public class PlankResource : MonoBehaviour
{
    [Header("Resource Settings")]
    [Tooltip("The initial number of planks the player starts with (collected from the ground).")]
    public int currentPlankCount = 0;

    [Header("Bridge Repair Status")]
    [Tooltip("The total number of gaps in the bridge that must be repaired.")]
    public int totalGapsToRepair = 4;
    private int gapsRepaired = 0;
    private bool bridgeRepaired = false;

    [Header("Final Trigger")]
    [Tooltip("Events that fire when the bridge is fully repaired (e.g., unlock a door, display message).")]
    public UnityEvent OnBridgeFullyRepaired;

    /// <summary>
    /// Attempts to use a specific number of planks to repair a bridge gap.
    /// </summary>
    /// <param name="amount">The number of planks to deduct.</param>
    /// <returns>True if planks were successfully deducted and used, otherwise False.</returns>
    public bool TryUsePlanks(int amount) // MODIFIED to accept an amount
    {
        if (currentPlankCount >= amount)
        {
            currentPlankCount -= amount;
            Debug.Log($"Planks used: {amount}. Planks remaining: {currentPlankCount}");
            return true;
        }
        else
        {
            Debug.Log($"Not enough planks. Need {amount}, have {currentPlankCount}.");
            return false;
        }
    }

    /// <summary>
    /// INCREASES the player's plank count by one. Called by PlankCollectible when picked up.
    /// </summary>
    public void AddPlank()
    {
        currentPlankCount++;
        Debug.Log($"Plank collected! Planks now held: {currentPlankCount}");
    }

    /// <summary>
    /// Called by a BridgeGap script when a plank has been successfully placed.
    /// </summary>
    public void GapRepaired()
    {
        if (bridgeRepaired) return;

        gapsRepaired++;
        Debug.Log($"Gap repaired! Total repaired: {gapsRepaired} / {totalGapsToRepair}");

        if (gapsRepaired >= totalGapsToRepair)
        {
            bridgeRepaired = true;
            Debug.Log("BRIDGE REPAIRED! Player can now cross.");
            OnBridgeFullyRepaired.Invoke();
        }
    }

    // Optional: Add a public method to display the count on a UI element
    public int GetPlankCount()
    {
        return currentPlankCount;
    }
}