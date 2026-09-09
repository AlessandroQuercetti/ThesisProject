using UnityEngine;
using RealisticEyeMovements;

public class ObjectLookController : MonoBehaviour
{
    [Header("Eye Movement Controller")]
    public LookTargetController lookController;

    [Header("Scene Objects")]
    public Transform magazine1;
    public Transform magazine2;
    public Transform remoteControl;
    public Transform tv;

    [Header("Settings")]
    public float lookDuration;

    private Coroutine currentLookRoutine;

    private void Start()
    {
        if (lookController == null)
        {
            Debug.LogWarning("ObjectLookController - LookTargetController missing");
        }
        
        if (magazine1 == null || magazine2 == null || remoteControl == null || tv == null)
        {
            Debug.LogWarning("ObjectLookController - One or more scene object references are missing");
        }

        if (lookDuration <= 0f)
        {
            Debug.LogWarning("ObjectLookController - Look duration should be greater than 0");
        }
    }

    // =====================================================
    // PUBLIC FUNCTION
    // =====================================================

    public void HandleLookTargets(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        text = text.ToLower();

        // MAGAZINES
        if (text.Contains("magazine") ||
                 text.Contains("journal") ||
                 text.Contains("newspaper"))
        {
            Transform target =
                Random.value > 0.5f ? magazine1 : magazine2;

            LookAtObject(target);
        }

        // REMOTE
        else if (text.Contains("remote") ||
                 text.Contains("controller"))
        {
            LookAtObject(remoteControl);
        }

        // TV
        else if (text.Contains("tv") ||
                 text.Contains("television"))
        {
            LookAtObject(tv);
        }
    }

    // =====================================================
    // LOOK LOGIC
    // =====================================================

    public void LookAtObject(Transform target)
    {
        if (target == null)
        {
            Debug.LogWarning("ObjectLookController - Target is NULL");
            return;
        }

        Debug.Log("ObjectLookController - LOOKING AT: " + target.name);

        // stop previous coroutine
        if (currentLookRoutine != null)
        {
            StopCoroutine(currentLookRoutine);
        }

        // start looking
        lookController.LookAtPoiDirectly(target, lookDuration);

        // return idle later
        currentLookRoutine = StartCoroutine(ReturnToIdleLook());
    }

    private System.Collections.IEnumerator ReturnToIdleLook()
    {
        yield return new WaitForSeconds(lookDuration);

        if (lookController != null)
        {
            lookController.LookAroundIdly();
        }
    }

    public void SetLookAtPlayerRatio(float newValue)
    {
        lookController.lookAtPlayerRatio = newValue;
    }
}