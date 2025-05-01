using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
public class ClimbInteractable : XRBaseInteractable
{
    [Header("Climbing Settings")]
    public float hapticIntensity = 0.5f;
    public float hapticDuration = 0.1f;

    private Climber climber;
    private XRBaseInteractor currentInteractor;

    protected override void Awake()
    {
        base.Awake();
        climber = FindObjectOfType<Climber>();
        SetupRigidbody();
        gameObject.layer = LayerMask.NameToLayer("Climbable");
    }

    private void SetupRigidbody()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (interactor is XRDirectInteractor)
        {
            currentInteractor = interactor;
            XRBaseController controller = interactor.GetComponent<XRBaseController>();

            if (controller != null && climber != null)
            {
                controller.SendHapticImpulse(hapticIntensity, hapticDuration);
                climber.StartClimbing(controller);
            }
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);

        if (currentInteractor == interactor && climber != null)
        {
            climber.StopClimbing();
            currentInteractor = null;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.LogWarning($"Added BoxCollider to {gameObject.name}", this);
        }
    }
#endif
}