using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;
using System.Collections;


public class ShowTvImageManager : MonoBehaviour
{
    [Header("Visual References")]
    public GameObject imageToShow;
    public GameObject panel;
    public TMP_Text loadingText;

    private XRGrabInteractable grabInteractable;
    private InputDevice rightController;

    private bool isHeld;
    private bool triggerWasPressed;
    private bool tvOn = false;
    private bool isTransitioning = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (imageToShow != null)
            imageToShow.SetActive(false);

        if (panel != null)
            panel.SetActive(false);
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
    }

    private void Update()
    {
        if (!isHeld)
            return;

        if (!rightController.isValid)
        {
            rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
            return;
        }

        bool triggerPressed;

        if (rightController.TryGetFeatureValue(CommonUsages.triggerButton, out triggerPressed))
        {
            // required to avoid toggling the image multiple times while holding the trigger
            if (triggerPressed && !triggerWasPressed)
            {
                ToggleTv();
            }

            triggerWasPressed = triggerPressed;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        triggerWasPressed = false;
    }

    private void ToggleTv()
    {
        if (isTransitioning)
            return;

        StartCoroutine(ToggleTvCoroutine());
    }

    private IEnumerator ToggleTvCoroutine()
    {
        isTransitioning = true;

        panel.SetActive(true);

        if (!tvOn)
        {
            // TURNING ON

            loadingText.text = ".";
            yield return new WaitForSeconds(0.3f);

            loadingText.text = ". .";
            yield return new WaitForSeconds(0.3f);

            loadingText.text = ". . .";
            yield return new WaitForSeconds(0.3f);

            imageToShow.SetActive(true);

            panel.SetActive(false);

            tvOn = true;
        }
        else
        {
            // TURNING OFF

            loadingText.text = ". . .";
            yield return new WaitForSeconds(0.3f);

            loadingText.text = ". .";
            yield return new WaitForSeconds(0.3f);

            loadingText.text = ".";
            yield return new WaitForSeconds(0.3f);

            imageToShow.SetActive(false);

            panel.SetActive(false);

            tvOn = false;
        }

        isTransitioning = false;
    }
}