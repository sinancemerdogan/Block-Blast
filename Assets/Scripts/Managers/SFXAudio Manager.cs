using System.Collections.Generic;
using UnityEngine;

public class SFXAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private AudioEventChannel _SFXEventChannel;

    private void Awake() 
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() 
    {
        _SFXEventChannel.OnAudioRequested += PlaySFX;
    }
    private void OnDisable() 
    {
        _SFXEventChannel.OnAudioRequested -= PlaySFX;
    }
    private void PlaySFX(AudioClip sfx) 
    {
        if(_audioSource == null) { Debug.Log("AudioSouce is null"); }
        _audioSource.PlayOneShot(sfx, 0.05f);
    }


    //Event-list approach
    //[SerializeField] private AudioClipListVariable _blockSFXQueue;

    //public void PlayBlockSFX() 
    //{
    //    if (_blockSFXQueue == null) {
    //        Debug.LogWarning("No SFX Queue found.");
    //        return;
    //    }

    //    List<AudioClip> blockSFXQueue = new(_blockSFXQueue.Items);

    //    foreach (var sfx in blockSFXQueue) {
    //        _audioSource.PlayOneShot(sfx, 0.05f);
    //        _blockSFXQueue.Items.Remove(sfx);
    //    }
    //    blockSFXQueue.Clear();
    //}
}
