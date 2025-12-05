using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineController : MonoBehaviour
{
    [Header("Settings")]
    public int vineID; // 0, 1, 2, or 3
    public Vector3 moveOffset = new Vector3(0, 5, 0); // Where it moves when opened
    public float moveSpeed = 2.0f;

    private Vector3 initialPosition;
    private Vector3 openPosition;
    private bool isMoved = false;
    private PuzzleManager manager;

    void Start()
    {
        // Remember where we started so we can reset later
        initialPosition = transform.position;
        openPosition = initialPosition + moveOffset;
        
        // Find the manager automatically
        manager = FindObjectOfType<PuzzleManager>();
    }

    private void OnMouseDown()
    {
        // Only allow clicking if it hasn't moved yet and the puzzle isn't solved
        if (!isMoved && manager != null)
        {
            manager.CheckVineClick(this);
        }
    }

    // Called by the Manager when this is the CORRECT vine
    public void OpenVine()
    {
        isMoved = true;
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(openPosition));
    }

    // Called by the Manager when the sequence is WRONG
    public void ResetVine()
    {
        isMoved = false;
        StopAllCoroutines();
        StartCoroutine(MoveToPosition(initialPosition));
    }

    // Smooth movement coroutine
    IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = target;
    }
}