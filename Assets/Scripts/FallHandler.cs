using UnityEngine;

public class FallZoneRelay : MonoBehaviour
{
    public enum ZoneType { Activator, Detector }

    [SerializeField] private ZoneType type;
    [SerializeField] private FallZonesManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (manager == null) return;

        // Find the root transform (XR rigs often have nested colliders)
        Transform root = other.attachedRigidbody
            ? other.attachedRigidbody.transform.root
            : other.transform.root;

        if (root != null && root.CompareTag(manager.PlayerTag))
        {
            if (type == ZoneType.Activator)
                manager.OnActivatorTriggered(root);
            else
                manager.OnDetectorTriggered(root);
        }
    }
}
