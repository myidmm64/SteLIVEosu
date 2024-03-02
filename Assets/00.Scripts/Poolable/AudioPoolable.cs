using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPoolable : PoolableObject
{
    private AudioSource _audioSource = null;
    private bool _isPlaying = false;

    public override void PopInit()
    {
    }

    public override void PushInit()
    {
    }

    public override void StartInit()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(SAudioData audioData)
    {
        _audioSource.clip = audioData.audioClip;
        _audioSource.pitch = audioData.pitch;
        _audioSource.volume = audioData.vol;
        _audioSource.Play();
        _isPlaying = true;
    }

    private void Update()
    {
        if (!_isPlaying) return;
        if(!_audioSource.isPlaying)
        {
            _isPlaying = false;
            PoolManager.Instance.Push(this);
        }
    }
}
