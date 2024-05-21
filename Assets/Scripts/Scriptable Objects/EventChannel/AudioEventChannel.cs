using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Audio Event Channel", menuName = "Scriptable Objects/Event Channels/Audio Event Channel")]
public class AudioEventChannel : ScriptableObject
{
    public UnityAction<AudioClip> OnAudioRequested;

    public void Raise(AudioClip clip) 
    {
        if (OnAudioRequested != null) 
        {
            OnAudioRequested.Invoke(clip);
        }
        else 
        {
            Debug.LogWarning("An audio was requested, but nobody picked it up.");
        }
    }
}
