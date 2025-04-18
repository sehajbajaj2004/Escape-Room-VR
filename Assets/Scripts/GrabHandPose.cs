using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandPose;

    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(SetupPose);
        }

        if (rightHandPose != null)
        {
            rightHandPose.gameObject.SetActive(false);
        }
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor interactor)
        {
            HandData handData = interactor.transform.GetComponentInChildren<HandData>();
            if (handData != null)
            {
                if (handData.animator != null)
                {
                    handData.animator.enabled = false;
                }

                SetHandDataValues(handData, rightHandPose);
                SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotations);
            }
        }
    }

    public void SetHandDataValues(HandData h1, HandData h2)
    {
        if (h1 == null || h2 == null) return;

        startingHandPosition = h1.root.localPosition;
        finalHandPosition = h2.root.localPosition;

        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        int boneCount = h1.fingerBones.Length;
        startingFingerRotations = new Quaternion[boneCount];
        finalFingerRotations = new Quaternion[boneCount];

        for (int i = 0; i < boneCount; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandData h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        if (h == null || h.fingerBones == null || newBonesRotation == null) return;

        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;

        for (int i = 0; i < newBonesRotation.Length && i < h.fingerBones.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }
}
