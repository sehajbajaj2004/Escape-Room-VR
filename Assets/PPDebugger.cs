using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PPDebugger : MonoBehaviour
{
    void Start()
    {
        var camData = GetComponent<UniversalAdditionalCameraData>();
        Debug.Log("Post Processing enabled on camera: " + camData.renderPostProcessing);

        var volumes = FindObjectsOfType<Volume>();
        foreach (var v in volumes)
        {
            Debug.Log("Volume found: " + v.name + " | IsGlobal: " + v.isGlobal);
            if (v.profile.TryGet(out Vignette vignette))
            {
                Debug.Log("Vignette intensity: " + vignette.intensity.value + " Active: " + vignette.active);
            }
        }
    }
}
