using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioSource _bgm = null;
    private Beatmap _beatmap = null;

    // Timer
    private int _currentIndex = 0;
    private double _currentTime = 0;
    private double _currentMs = 0;

    // Create HitObject Setting
    private List<HitObjectData> _hitObjectDatas = new List<HitObjectData>();
    private Vector3 _circleSize;
    private double _approachDuration = 0;
    private double _fadeinDuration = 0;

    public void PlayBeatmap()
    {
        _bgm.Play();
        string songName = DevelopHelperObj.Instance.songSelectDropdown.options
            [DevelopHelperObj.Instance.songSelectDropdown.value].text;
        _beatmap = new Beatmap(songName);
        _beatmap.LoadOsuFile();

        _hitObjectDatas = _beatmap.osuFile.hitObject.hitObjectDatas;
        float r = 1f - 0.082f * _beatmap.osuFile.difficulty.circleSize;
        _circleSize = new Vector3(r, r);
        _approachDuration = GetApproachAnimationDuration(_beatmap.osuFile.difficulty.approachRate);
        _fadeinDuration = GetHitObjectFadeInDuration(_beatmap.osuFile.difficulty.approachRate);
    }

    public double GetApproachAnimationDuration(float ar)
    {
        if (ar < 5)
            return 1200 + 600 * (5 - ar) / 5;
        else if (ar == 5)
            return 1200;
        else if (ar > 5)
            return 1200 - 750 * (ar - 5) / 5;
        return 0;
    }

    public double GetHitObjectFadeInDuration(float ar)
    {
        if (ar < 5)
            return 800 + 400 * (5 - ar) / 5;
        else if (ar == 5)
            return 800;
        else if (ar > 5)
            return 800 - 500 * (ar - 5) / 5;
        return 0;
    }

    private void Update()
    {
    }

    private bool IsInputStarted()
    {
        List<KeyCode> keys = new List<KeyCode>() { KeyCode.Z, KeyCode.X, KeyCode.Mouse0, KeyCode.Mouse1 };
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key)) return true;
        }
        return false;
    }
}
