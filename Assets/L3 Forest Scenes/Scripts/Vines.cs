using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineClickUntangle : MonoBehaviour
{
    [Header("Untangle Settings")]
    [Tooltip("The direction in which the vine will move when clicked.")]
    public Vector3 moveDirection = new Vector3(0, 2, -2);

    [Tooltip("How far the vine moves away when untangling.")]
    public float moveDistance = 2f;

    [Tooltip("How long (in seconds) the movement takes.")]
    public float moveDuration = 1.5f;

    [Tooltip("If true, adds a small random offset to the move direction for a more natural effect.")]
    public bool randomizeDirection = true;

    private bool isClicked = false;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveTimer = 0f;
    private bool isMoving = false;

    void OnMouseDown()
    {
        if (isClicked) return; // Prevent re-clicking
        isClicked = true;

        // Disable collider to make it unclickable
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        startPosition = transform.position;

        // Optional: randomize direction slightly
        Vector3 finalDir = moveDirection.normalized;
        if (randomizeDirection)
        {
            finalDir += new Vector3(
                Random.Range(-0.3f, 0.3f),
                Random.Range(-0.3f, 0.3f),
                Random.Range(-0.3f, 0.3f)
            );
        }

        targetPosition = startPosition + finalDir.normalized * moveDistance;
        moveTimer = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        moveTimer += Time.deltaTime;
        float t = moveTimer / moveDuration;

        // Smooth easing out (starts fast, ends slow)
        t = Mathf.Clamp01(t);
        float easedT = EaseOutCubic(t);

        transform.position = Vector3.Lerp(startPosition, targetPosition, easedT);

        if (t >= 1f)
        {
            isMoving = false;
        }
    }

    // Easing function: cubic ease-out
    private float EaseOutCubic(float x)
    {
        return 1 - Mathf.Pow(1 - x, 3);
    }
}


