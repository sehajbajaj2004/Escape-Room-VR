using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClimbInteractable : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        ClimbProvider climbProvider = args.interactorObject.transform.GetComponent<ClimbProvider>();
        if (climbProvider != null)
        {
            climbProvider.StartClimbing();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        ClimbProvider climbProvider = args.interactorObject.transform.GetComponent<ClimbProvider>();
        if (climbProvider != null)
        {
            climbProvider.StopClimbing();
        }
    }
}
