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
    public bool fuelFilled = true;

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

    // NEW: Animation delay
    private float animationDelay = 5f;

    void Start()
    {
        steeringWheelSocket.selectEntered.AddListener(OnSteeringWheelPlaced);
        keySocket.selectEntered.AddListener(OnKeyPlaced);

        pushButton.onPress.AddListener(OnButtonPressed);

        if (boatRigidbody != null)
            boatRigidbody.isKinematic = true;

        // UI initialization
        if (steeringWheelCheckUI) steeringWheelCheckUI.SetActive(false);
        if (keyCheckUI) keyCheckUI.SetActive(false);
        if (fuelCheckUI) fuelCheckUI.SetActive(true); // fuel already full
    }

    void OnSteeringWheelPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("steering wheel"))
        {
            steeringWheelSnapped = true;
            if (steeringWheelCheckUI) steeringWheelCheckUI.SetActive(true);
        }
    }

    void OnKeyPlaced(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.CompareTag("key"))
        {
            keySnapped = true;
            if (keyCheckUI) keyCheckUI.SetActive(true);
        }
    }

    public void FuelFilled()
    {
        fuelFilled = true;
        if (fuelCheckUI) fuelCheckUI.SetActive(true);
    }

    void OnButtonPressed()
    {
        if (AllConditionsMet())
        {
            Debug.Log("All conditions met — starting delayed animation sequence.");
            StartCoroutine(DelayedActivationSequence());
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

    IEnumerator DelayedActivationSequence()
    {
        // ⏳ WAIT 5 seconds BEFORE playing animation
        yield return new WaitForSeconds(animationDelay);

        // 🎬 Trigger animation
        deckAnim1.SetTrigger(triggerName);
        deckAnim2.SetTrigger(triggerName);
        Debug.Log("Ship deck animation triggered!");

        // 🚤 Enable boat physics (1 second after animation → 1 + 5 = 6 seconds total)
        yield return new WaitForSeconds(1f);
        if (boatRigidbody != null)
        {
            boatRigidbody.isKinematic = false;
            Debug.Log("Boat physics activated!");
        }

        // 🌊 Load scene after original delay + extra 5 seconds
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(10);
    }
}
