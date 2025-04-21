using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ShapePlacementManager : MonoBehaviour
{
    public XRSocketInteractor triangleSocket;
    public XRSocketInteractor squareSocket;
    public XRSocketInteractor circleSocket;

    public Animator boxAnimator;

    private void Start()
    {
        triangleSocket.selectEntered.AddListener(OnSocketUpdated);
        squareSocket.selectEntered.AddListener(OnSocketUpdated);
        circleSocket.selectEntered.AddListener(OnSocketUpdated);
    }

    private void OnSocketUpdated(SelectEnterEventArgs args)
    {
        CheckAllShapesPlaced();
    }

    void CheckAllShapesPlaced()
    {
        bool trianglePlaced = IsCorrectObjectInSocket(triangleSocket, "Triangle");
        bool squarePlaced = IsCorrectObjectInSocket(squareSocket, "Cube");
        bool circlePlaced = IsCorrectObjectInSocket(circleSocket, "Cylinder");

        if (trianglePlaced && squarePlaced && circlePlaced)
        {
            boxAnimator.SetTrigger("OpenBox");
        }
    }

    bool IsCorrectObjectInSocket(XRSocketInteractor socket, string expectedTag)
    {
        if (socket.hasSelection)
        {
            var interactable = socket.GetOldestInteractableSelected();
            if (interactable != null && interactable.transform.CompareTag(expectedTag))
            {
                return true;
            }
        }
        return false;
    }
}
