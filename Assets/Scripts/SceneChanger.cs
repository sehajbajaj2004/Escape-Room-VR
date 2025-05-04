using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(LoadSceneAsync());
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        // Save current game state before changing scene
        GameManager.Instance?.SaveGameState();

        // Unload current scene
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation unload = SceneManager.UnloadSceneAsync(currentScene);
        yield return unload;

        // Clean unused assets to avoid lighting/artifacts issues
        yield return Resources.UnloadUnusedAssets();
        yield return new WaitForSeconds(0.5f);

        // Load new scene additively or normally
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Single);
        yield return load;

        // Optionally re-apply lighting settings if needed
        DynamicGI.UpdateEnvironment();
    }
}
