using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Collider))]
public class ClimbInteractable : XRBaseInteractable
{
    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);

        if (interactor is XRDirectInteractor)
        {
            XRController controller = interactor.GetComponent<XRController>();
            if (controller != null)
            {
                Climber.climbingHand = controller;
                SendHapticImpulse(controller, 0.5f, 0.1f);
            }
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);

        if (Climber.climbingHand != null &&
            Climber.climbingHand.gameObject == interactor.gameObject)
        {
            Climber.climbingHand = null;
        }
    }

    private void SendHapticImpulse(XRController controller, float amplitude, float duration)
    {
        controller.SendHapticImpulse(amplitude, duration);
    }

    private void OnValidate()
    {
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.LogWarning("Added BoxCollider to " + gameObject.name);
        }
    }
}