using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class SprintController : MonoBehaviour
{
    public DynamicMoveProvider moveProvider;
    public float normalSpeed = 1.5f;
    public float sprintSpeed = 4f;
    public float maxBoostDuration = 5f;
    public float cooldownDuration = 10f;

    private InputActionProperty leftGripAction;
    private InputActionProperty rightGripAction;

    private bool isBoosting = false;
    private bool isCooldown = false;

    private float boostTimer = 0f;
    private float cooldownTimer = 0f;
    private float currentBoostTime;

    private void Start()
    {
        // Left and Right Grip bindings
        leftGripAction = new InputActionProperty(new InputAction(type: InputActionType.Button, binding: "<XRController>{LeftHand}/gripPressed"));
        rightGripAction = new InputActionProperty(new InputAction(type: InputActionType.Button, binding: "<XRController>{RightHand}/gripPressed"));

        leftGripAction.action.Enable();
        rightGripAction.action.Enable();

        currentBoostTime = maxBoostDuration;
    }

    private void Update()
    {
        float leftGrip = leftGripAction.action.ReadValue<float>();
        float rightGrip = rightGripAction.action.ReadValue<float>();

        bool bothGripsHeld = (leftGrip > 0.5f) && (rightGrip > 0.5f);

        if (bothGripsHeld && !isCooldown && currentBoostTime > 0f)
        {
            if (!isBoosting)
            {
                StartBoost();
            }
            else
            {
                boostTimer += Time.deltaTime;
                currentBoostTime -= Time.deltaTime;

                if (currentBoostTime <= 0f)
                {
                    currentBoostTime = 0f;
                    StopBoost();
                    StartCooldown();
                }
            }
        }
        else
        {
            if (isBoosting)
            {
                StopBoost();
            }

            if (isCooldown)
            {
                cooldownTimer += Time.deltaTime;
                if (cooldownTimer >= cooldownDuration)
                {
                    EndCooldown();
                }
            }
        }
    }

    private void StartBoost()
    {
        isBoosting = true;
        moveProvider.moveSpeed = sprintSpeed;
        boostTimer = 0f;
    }

    private void StopBoost()
    {
        isBoosting = false;
        moveProvider.moveSpeed = normalSpeed;
    }

    private void StartCooldown()
    {
        isCooldown = true;
        cooldownTimer = 0f;
    }

    private void EndCooldown()
    {
        isCooldown = false;
        currentBoostTime = maxBoostDuration;
    }
}
