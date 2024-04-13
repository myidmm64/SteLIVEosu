using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

using OsuParsers.Beatmaps;
using OsuParsers.Decoders;
using OsuParsers.Beatmaps.Objects;
using OsuParsers.Database.Objects;

public class BeatmapPlayer : MonoSingleTon<BeatmapPlayer>
{
    [SerializeField]
    private float _offset = 0;
    private Beatmap _currentBeatmap = null;

    [SerializeField]
    private Player _player = null;
    [SerializeField]
    private AudioSource _bgm = null;

    // Timer
    private int _currentIndex = 0;
    private double _currentTime = 0;
    private double _currentMs = 0;
    public double CurrentMs => _currentMs;

    private HitObject _currentHitObjData = null;
    [SerializeField]
    private Queue<HitObject_Game> _hitObjects = new Queue<HitObject_Game>();

    private bool _isPlaying = false;

    public void PlayBeatmap()
    {
        string osuFileKey = DevelopHelperObj.Instance.songSelectDropdown.options
            [DevelopHelperObj.Instance.songSelectDropdown.value].text;
        SelectOsuFileData selectedFileData = DevelopHelperObj.Instance._developTestSelectOsuFileMap[osuFileKey];
        _currentBeatmap = selectedFileData.beatmap;

        _currentIndex = 0;
        _currentTime = 0;
        _currentMs = 0;

        _bgm.Stop();
        _bgm.clip = SongUtility.LoadBGM(selectedFileData);
        double leadinSec = _currentBeatmap.GeneralSection.AudioLeadIn * 0.001f;
        double scheduledTime = AudioSettings.dspTime + leadinSec;
        _bgm.PlayScheduled(scheduledTime);

        _currentHitObjData = _currentBeatmap.HitObjects[0];

        _isPlaying = true;
    }

    public void CleanHitObjectQueue()
    {
        if (_hitObjects.Count == 0) return;
        while (true)
        {
            if (_hitObjects.Count == 0) break;
            HitObject_Game hitObject = _hitObjects.Peek();
            if (hitObject == null) break;
            if (hitObject.IsDisable() == false)
                break;
            _hitObjects.Dequeue();
            PoolManager.Instance.Push(hitObject);
        }
    }

    private void Update()
    {
        ReadBeatmap();
    }

    private void ReadBeatmap()
    {
        CleanHitObjectQueue();
        _currentTime += Time.deltaTime;
        _currentMs = TimeSpan.FromSeconds(_currentTime).TotalMilliseconds;

        if (_isPlaying == false) return;

        if (_currentHitObjData.StartTime - _currentBeatmap.GetAnimationPreemptDuration() + _offset < _currentMs)
        {
            // Create
            HitObject_Game hitObject = PoolManager.Instance.Pop(EPoolType.HitObject) as HitObject_Game;
            hitObject.Init(_currentHitObjData, _currentBeatmap, _currentMs, _offset);
            _hitObjects.Enqueue(hitObject);

            _currentIndex++;
            if (_currentIndex < _currentBeatmap.HitObjects.Count)
            {
                _currentHitObjData = _currentBeatmap.HitObjects[_currentIndex];
            }
            else
            {
                _isPlaying = false;
            }
        }
    }

    public void Collect()
    {
        if (_hitObjects.Count == 0) return;

        CleanHitObjectQueue();

        foreach (var hitObject in _hitObjects)
        {
            if (_player.cursorObject.IsCollectedObject(hitObject.transform.position))
            {
                if (hitObject != _hitObjects.Peek())
                {
                    hitObject.ShakeAnimation();
                    return;
                }

                EJudgement judgement = hitObject.CollectObject(_currentMs);
                if (judgement == EJudgement.None)
                {
                    hitObject.ShakeAnimation();
                    return;
                }
                return;
            }
        }
    }

    /// <summary>
    /// 현재 맵에 있는 모든 HitObject와의 거리를 Draw
    /// </summary>
    private void OnDrawGizmos()
    {
        foreach (var hitObject in _hitObjects)
        {
            if (_player.cursorObject.IsCollectedObject(hitObject.transform.position))
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawLine(_player.cursorObject.transform.position, hitObject.transform.position);
        }
    }
}
