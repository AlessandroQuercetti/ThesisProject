using CrazyMinnow.SALSA;
using UnityEngine;

public class AmplitudeDetector : MonoBehaviour
{
    [Header("Salsa")]
    public Salsa salsa; 

    [Header("Animation")]
    public CharacterAnimationManager animationManager;

    [Header("Object Look")]
    public ObjectLookController objectLookController;

    [Header("Detection")]
    public float activationThreshold = 0.01f;
    public float releaseDelay = 2f;

    private bool isTalking;
    private float lastVoiceTime;

    private void Update()
    {
        float level = salsa.CachedAnalysisValue;

        if (level > activationThreshold)
        {
            lastVoiceTime = Time.time;

            if (!isTalking)
            {
                isTalking = true;
                animationManager.StartTalking();
                objectLookController.SetLookAtPlayerRatio(0.65f);
            }
        }
        else
        {
            if (isTalking && Time.time - lastVoiceTime > releaseDelay)
            {
                isTalking = false;
                animationManager.StopTalking();
                objectLookController.SetLookAtPlayerRatio(0.90f);
            }
        }
    }
}