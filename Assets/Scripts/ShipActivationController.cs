using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.SceneManagement;
using System.Collections;

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
    public Rigidbody boatRigidbody;

    [Header("Scene Change Delay (Seconds)")]
    public float delayBeforeSceneChange = 3f;

    [Header("UI Checkboxes")]
    public GameObject steeringWheelCheckUI;
    public GameObject keyCheckUI;
    public GameObject fuelCheckUI;

    void Start()
    {
        steeringWheelSocket.selectEntered.AddListener(OnSteeringWheelPlaced);
        keySocket.selectEntered.AddListener(OnKeyPlaced);

        pushButton.onPress.AddListener(OnButtonPressed);

        if (boatRigidbody != null)
            boatRigidbody.isKinematic = true;

        // --- Initialize UI ---
        if (steeringWheelCheckUI) steeringWheelCheckUI.SetActive(false);
        if (keyCheckUI) keyCheckUI.SetActive(false);

        // Fuel is already filled → show it as checked
        if (fuelCheckUI) fuelCheckUI.SetActive(true);
    }

    void OnSteeringWheelPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("steering wheel"))
        {
            steeringWheelSnapped = true;

            // Enable steering wheel checkmark UI
            if (steeringWheelCheckUI)
                steeringWheelCheckUI.SetActive(true);
        }
    }

    void OnKeyPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("key"))
        {
            keySnapped = true;

            // Enable key checkmark UI
            if (keyCheckUI)
                keyCheckUI.SetActive(true);
        }
    }

    public void FuelFilled()
    {
        fuelFilled = true;
        Debug.Log("Fuel has been filled!");

        // Enable fuel checkmark UI
        if (fuelCheckUI)
            fuelCheckUI.SetActive(true);
    }

    void OnButtonPressed()
    {
        if (AllConditionsMet())
        {
            // Play deck animations immediately
            deckAnim1.SetTrigger(triggerName);
            deckAnim2.SetTrigger(triggerName);

            // Delay the boat physics activation by 1 second
            StartCoroutine(EnableBoatPhysicsAfterDelay());

            // Start delayed scene change
            StartCoroutine(ChangeSceneAfterDelay());

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

    IEnumerator EnableBoatPhysicsAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (boatRigidbody != null)
        {
            boatRigidbody.isKinematic = false;
            Debug.Log("Boat physics activated after delay!");
        }
    }

    IEnumerator ChangeSceneAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(7);
    }
}
