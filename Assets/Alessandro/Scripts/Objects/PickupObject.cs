using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PickupObject : MonoBehaviour
{
    [Header("Pickup Reference")]
    public Transform heldOffset;

    [Header("Eye Movement Controller")]
    public ObjectLookController lookController;

    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Debug.Log("Picked up: " + gameObject.name);

        transform.localPosition = heldOffset.localPosition;
        transform.localRotation = heldOffset.localRotation;

        lookController.LookAtObject(transform);
    }
}