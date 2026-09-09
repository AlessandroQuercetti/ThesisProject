using Meta.WitAi.Json;
using Oculus.Voice;
using TMPro;
using UnityEngine;


public class LiveTranscriptionUI : MonoBehaviour
{
    public AppVoiceExperience appVoice;

    [Header("UI Elements")]
    public GameObject recordingImage;
    public GameObject pausedImage;
    public TMP_Text transcriptionText;

    public LiveTransctiptionUIConfig config;

    // =====================================================
    // UNITY
    // =====================================================

    private void OnEnable()
    {
        appVoice.VoiceEvents.OnResponse.AddListener(OnResponse);
        appVoice.VoiceEvents.OnError.AddListener(OnError);

        config.OnLiveTransctiptionToggle += ToggleImage;
    }

    private void OnDisable()
    {
        appVoice.VoiceEvents.OnResponse.RemoveListener(OnResponse);
        appVoice.VoiceEvents.OnError.RemoveListener(OnError);

        config.OnLiveTransctiptionToggle -= ToggleImage;
    }

    private void ToggleImage (bool isListening)
    {
        recordingImage.SetActive(isListening);
        pausedImage.SetActive(!isListening);
    }


    // =====================================================
    // WIT CALLBACKS
    // =====================================================

    private void OnResponse(WitResponseNode response)
    {
        string text = response["text"];

        if (!string.IsNullOrEmpty(text))
        {
            transcriptionText.text = text;
        }
    }

    private void OnError(string error, string message)
    {
        Debug.LogError("LiveTranscriptionUI - " + error + " | " + message);
        transcriptionText.text = "Error...";
    }


}