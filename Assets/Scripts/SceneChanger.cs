using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad; // Name of the scene you want to load

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the collider is the XR Rig (Player)
        if (other.CompareTag("Player"))
        {
            LoadScene();
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
