using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayThemeAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField] private AudioEvent _gameplayThemePlayer;
    [SerializeField] private AudioEvent _levelEndAudioPlayer;
    [SerializeField] private BooleanVariable _isLevelWon;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Stop();
    }

    public void PlayGameplayTheme() 
    {
        if (_gameplayThemePlayer == null) 
        {
            Debug.LogWarning("Cannot find a gameplay theme player.");
            Debug.LogWarning("No gameplay theme will be played.");
            return;
        }
        if (_audioSource.isPlaying) return;
        _audioSource.loop = true;
        _gameplayThemePlayer.Play(_audioSource);
    }

    public void PlayLevelEndClip() 
    {
        if (_levelEndAudioPlayer == null) {
            Debug.LogWarning("Cannot find a level end audio player.");
            Debug.LogWarning("No level end audio will be played.");
            return;
        }

        if (_levelEndAudioPlayer is LevelEndAudioClipPlayer player) 
        {
            player.SetClip(_isLevelWon.Value);
            player.Play(_audioSource);
        }
    }
}
