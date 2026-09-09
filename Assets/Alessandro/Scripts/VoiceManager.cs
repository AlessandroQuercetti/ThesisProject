using Meta.WitAi.Json;
using Oculus.Voice;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{

    [Header("Voice")]
    public AppVoiceExperience appVoice;

    [Header("Animation")]
    public CharacterAnimationManager animationManager;

    [Header("Object Look")]
    public ObjectLookController objectLookController;

    [Header("config")]
    public LiveTransctiptionUIConfig config;

    private bool isListening = false;

    // =====================================================
    // UNITY
    // =====================================================

    private void Start()
    {
        if (appVoice == null)
        {
            Debug.LogError("VoiceManager - AppVoiceExperience reference is missing!");
        }
        if (animationManager == null)
        {
            Debug.LogError("VoiceManager - CharacterAnimationManager reference is missing!");
        }
        if (objectLookController == null)
        {
            Debug.LogError("VoiceManager - ObjectLookController reference is missing!");
        }
    }


    private void OnEnable()
    {
        appVoice.VoiceEvents.OnResponse.AddListener(OnResponse);
        appVoice.VoiceEvents.OnError.AddListener(OnError);
    }

    private void OnDisable()
    {
        appVoice.VoiceEvents.OnResponse.RemoveListener(OnResponse);
        appVoice.VoiceEvents.OnError.RemoveListener(OnError);
    }

    // =====================================================
    // LISTENING CONTROL
    // =====================================================

    public void ToggleListening()
    {
        isListening = !isListening;
        config.InvokeLiveTranscription(isListening);

        if (isListening)
        {
            StartListening();
        }
        else
        {
            StopListening();
        }
    }

    public void StartListening()
    {
        Debug.Log("VoiceManager - START LISTENING");

        //objectLookController.SetLookAtPlayerRatio(0.65f);

        appVoice.Activate();

        //animationManager.StartTalking(); commented because now it's managed by AumplitudeDetector

    }

    public void StopListening()
    {
        Debug.Log("VoiceManager - STOP LISTENING");

        appVoice.Deactivate();

        //objectLookController.SetLookAtPlayerRatio(0.90f);

        //animationManager.StopTalking();

    }

    // =====================================================
    // WIT CALLBACKS
    // =====================================================

    private void OnResponse(WitResponseNode response)
    {
        Debug.Log("VoiceManager - Voice response received");

        string text = response["text"];

        if (!string.IsNullOrEmpty(text))
        {
            Debug.Log(text);

            // LOOK AT OBJECTS
            objectLookController.HandleLookTargets(text);
        }
    }

    private void OnError(string error, string message)
    {
        Debug.LogWarning($"VoiceManager - Voice error: {error}");
        //animationManager.StopTalking();
    }
}