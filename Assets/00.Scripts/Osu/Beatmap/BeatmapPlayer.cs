using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _bgm = null;

    public void PlayBeatmap()
    {
        _bgm.Play();
    }
}
