using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioPool
{
    private static AudioDB _audioDB = null;
    private const string AUDIO_DB_PATH = "SO\\AudioDB";

    public static AudioPoolable PopAudio(EAudioType audioType)
    {
        if(_audioDB == null)
            _audioDB = Resources.Load(AUDIO_DB_PATH) as AudioDB;
        if(_audioDB == null)
        {
            Debug.LogError($"Resources\\{AUDIO_DB_PATH} ��ο� AudioDB�� �������� �ʽ��ϴ�.");
        }
        if(PoolManager.Instance == null)
        {
            Debug.LogError($"PoolManager�� �������� �ʽ��ϴ�.");
        }
        AudioPoolable audioPoolable = PoolManager.Instance.Pop(EPoolType.Audio) as AudioPoolable;
        foreach(SAudioData audioData in _audioDB.audioDatas)
        {
            if(audioData.audioType == audioType)
            {
                audioPoolable.PlayAudio(audioData);
                return audioPoolable;
            }
        }

        return null;
    }
}
