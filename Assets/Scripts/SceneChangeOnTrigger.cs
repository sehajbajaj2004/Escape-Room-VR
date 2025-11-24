using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeOnTrigger : MonoBehaviour
{
    [Header("Scene To Load")]
    public int sceneIndex = 5;    // default is Scene 5

    [Header("Player Tag")]
    public string playerTag = "Player";  // make sure XR Origin is tagged "Player"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}
