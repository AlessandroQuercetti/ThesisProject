using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LiveTransctiptionUIConfig", menuName = "Scriptable Objects/LiveTransctiptionUIConfig")]
public class LiveTransctiptionUIConfig : ScriptableObject
{
    public Action<bool> OnLiveTransctiptionToggle;

    public void InvokeLiveTranscription(bool flag)
    {
        OnLiveTransctiptionToggle?.Invoke(flag);
    }
}
