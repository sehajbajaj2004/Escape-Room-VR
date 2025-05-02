using UnityEngine;

public class CanvasTimer : MonoBehaviour
{
    public GameObject canvasToShow;   // Assign this in the inspector
    public float displayTime = 15f;

    private void Start()
    {
        StartCoroutine(ShowCanvasTemporarily());
    }

    private System.Collections.IEnumerator ShowCanvasTemporarily()
    {
        canvasToShow.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        canvasToShow.SetActive(false);
    }
}
