using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;

public class ShipActivationController : MonoBehaviour
{
    [Header("Socket Requirements")]
    public XRSocketInteractor steeringWheelSocket;
    public XRSocketInteractor keySocket;

    private bool steeringWheelSnapped = false;
    private bool keySnapped = false;

    [Header("Fuel Requirement")]
    public bool fuelFilled = true;   // TRUE by default

    [Header("XR Push Button")]
    public XRPushButton pushButton;

    [Header("Ship Deck Animations")]
    public Animator deckAnim1;
    public Animator deckAnim2;
    public string triggerName = "Open";

    [Header("Boat Rigidbody")]
    public Rigidbody boatRigidbody;  // Assign in inspector

    void Start()
    {
        steeringWheelSocket.selectEntered.AddListener(OnSteeringWheelPlaced);
        keySocket.selectEntered.AddListener(OnKeyPlaced);

        pushButton.onPress.AddListener(OnButtonPressed);

        // Ensure initial state
        if (boatRigidbody != null)
            boatRigidbody.isKinematic = true;
    }

    // ---------------- SOCKET EVENTS ----------------

    void OnSteeringWheelPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("steering wheel"))
        {
            steeringWheelSnapped = true;
        }
    }

    void OnKeyPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("key"))
        {
            keySnapped = true;
        }
    }

    // ---------------- OPTIONAL FUEL FUNCTION ----------------
    public void FuelFilled()
    {
        fuelFilled = true;
        Debug.Log("Fuel has been filled!");
    }

    // ---------------- PUSH BUTTON ----------------

    void OnButtonPressed()
    {
        if (AllConditionsMet())
        {
            // Play deck animations
            deckAnim1.SetTrigger(triggerName);
            deckAnim2.SetTrigger(triggerName);

            // Enable boat physics
            if (boatRigidbody != null)
            {
                boatRigidbody.isKinematic = false;
                Debug.Log("Boat physics activated!");
            }

            Debug.Log("Ship deck opening triggered!");
        }
        else
        {
            Debug.Log("Cannot activate — all conditions not met.");
        }
    }

    bool AllConditionsMet()
    {
        return steeringWheelSnapped && keySnapped && fuelFilled;
    }
}
