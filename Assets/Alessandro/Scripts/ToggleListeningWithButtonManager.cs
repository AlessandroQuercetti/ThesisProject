using UnityEngine;
using UnityEngine.XR;
using UnityEngine.InputSystem;

/// <summary>
/// This script allows you to toggle the voice manager's listening state using either the space key on the keyboard or the primary button (X) on the left VR controller.
/// 

public class ToggleListeningWithButtonManager : MonoBehaviour
{
    private UnityEngine.XR.InputDevice leftController;

    private bool lastButtonState = false;

    public VoiceManager voiceManager;

    private void Start()
    {
        leftController =
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
    }

    private void Update()
    {
        // SPACE KEY
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            //Debug.Log("SPACE");

            voiceManager.ToggleListening();
        }

        // XR CONTROLLER
        if (!leftController.isValid)
        {
            leftController =
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        }

        bool buttonPressed;

        if (leftController.TryGetFeatureValue(
            UnityEngine.XR.CommonUsages.primaryButton,
            out buttonPressed))
        {
            if (buttonPressed && !lastButtonState)
            {
                //Debug.Log("X BUTTON");

                voiceManager.ToggleListening();
            }

            lastButtonState = buttonPressed;
        }
    }
}