using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [Header("Puzzle Configuration")]
    // The specific order of IDs required to solve the puzzle
    // Example: 2, 0, 3, 1 means click Vine 2, then Vine 0, etc.
    public List<int> correctSequence; 
    
    [Header("Debug")]
    [SerializeField] private int currentSequenceIndex = 0; // Tracks progress

    // When a vine is clicked, it calls this function
    public void CheckVineClick(VineController clickedVine)
    {
        // 1. Check if the clicked vine matches the ID expected in the sequence
        if (clickedVine.vineID == correctSequence[currentSequenceIndex])
        {
            // CORRECT CLICK
            clickedVine.OpenVine();
            currentSequenceIndex++;

            // Check if puzzle is complete
            if (currentSequenceIndex >= correctSequence.Count)
            {
                Debug.Log("Puzzle Solved! The gate is open.");
                // Add code here to trigger the next event (e.g., load scene, play sound)
            }
        }
        else
        {
            // WRONG CLICK
            Debug.Log("Wrong vine! Resetting puzzle.");
            ResetPuzzle();
        }
    }

    private void ResetPuzzle()
    {
        currentSequenceIndex = 0;

        // Find all vines and tell them to go back to start
        VineController[] allVines = FindObjectsOfType<VineController>();
        foreach (VineController vine in allVines)
        {
            vine.ResetVine();
        }
    }
}