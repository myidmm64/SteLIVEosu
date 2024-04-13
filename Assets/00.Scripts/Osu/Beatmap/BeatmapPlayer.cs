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
    private CursorObject _cursor = null;
    [SerializeField]
    private AudioSource _bgm = null;

    // Timer
    private int _currentIndex = 0;
    private double _currentTime = 0;
    private double _currentMs = 0;
    public double CurrentMs => _currentMs;

    // Create HitObject Setting
    private const int _createOffset = 50;

    private double _preemptDuration = 0;
    private double _fadeinDuration = 0;
    private float _circleRadius = 0f;

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

        _preemptDuration = BeatmapUtility.GetApproachAnimationDuration(_currentBeatmap);
        _fadeinDuration = BeatmapUtility.GetHitObjectFadeInDuration(_currentBeatmap);
        _circleRadius = BeatmapUtility.GetCircleRadius(_currentBeatmap);

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

    }

    private void Update()
    {
        if (_isPlaying == false) return;

        _currentTime += Time.deltaTime;
        _currentMs = TimeSpan.FromSeconds(_currentTime).TotalMilliseconds;

        if (_currentHitObjData.StartTime - _preemptDuration - _createOffset + _offset < _currentMs)
        {
            // Create
            HitObject_Game hitObject = PoolManager.Instance.Pop(EPoolType.HitObject) as HitObject_Game;
            Vector2 hitObjectPosition = OsuPlayField.Instance.OsuPixelToWorldPosition(new Vector2(_currentHitObjData.Position.X, _currentHitObjData.Position.Y));
            Vector3 circleSize = Vector2.one * _circleRadius;
            hitObject.Init(_currentHitObjData, this, hitObjectPosition, circleSize, _preemptDuration, _fadeinDuration);
            _hitObjects.Enqueue(hitObject);

            _currentIndex++;
            _currentHitObjData = _currentBeatmap.HitObjects[_currentIndex];
        }

        /*
        foreach (var hitObject in _currentBeatmap.HitObjects)
        {
            if (hitObject is Slider)
            {
                Slider slider = (Slider)hitObject;
                Debug.Log(slider.SliderPoints);
            }
            else if (hitObject is OsuParsers.Beatmaps.Objects.HitCircle)
            {
                Debug.Log(hitObject.StartTime + " " + hitObject.EndTime + " " + hitObject.Position);
            }
        }
        return;
        */
        /*
        if(_hitObjects.Count > 0)
        {
            _hitObjects.Peek().TestColor(Color.red);
        }

        if (!_bgm.isPlaying) return;
        if (_currentIndex >= _hitObjectDatas.Count) return;

        _currentTime = _bgm.time;
        _currentMs = TimeSpan.FromSeconds(_currentTime).TotalMilliseconds;
        if (_hitObjectDatas[_currentIndex].hitTime - _preemptDuration + _offset < _currentMs)
        {
            HitObject hitObject = PoolManager.Instance.Pop(EPoolType.HitObject) as HitObject;
            Vector2 hitObjectPosition = _playField.OsuPixelToWorldPosition(new Vector2Int(_hitObjectDatas[_currentIndex].x, _hitObjectDatas[_currentIndex].y));
            Vector2 hitObjectScale = Vector2.one * BeatmapUtility.GetCircleRadius(_beatmap);
            hitObject.Init(this, _hitObjectDatas[_currentIndex].hitTime + _offset, hitObjectPosition, hitObjectScale);
            _currentIndex++;
            _hitObjects.Enqueue(hitObject);
        }
        */
    }

    public void OnHit(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            foreach (var hitObject in _hitObjects)
            {
                // 체크
                if (_cursor.IsCollectedObject(hitObject.transform.position))
                {
                    /*
                    if(hitObject != _hitObjects.Peek())
                    {
                        hitObject.ShakeAnimation();
                        return;
                    }
                    */

                    EJudgement judgement = hitObject.JudgementCalculate(_currentMs);
                    if (judgement == EJudgement.None)
                    {
                        hitObject.ShakeAnimation();
                        return;
                    }
                    return;
                }
            }
        }
    }

    public void DequeueObject()
    {
        if (_hitObjects.Count == 0) return;
        _hitObjects.Dequeue();
    }

    /// <summary>
    /// 현재 맵에 있는 모든 HitObject와의 거리를 Draw
    /// </summary>
    private void OnDrawGizmos()
    {
        foreach (var hitObject in _hitObjects)
        {
            if (_cursor.IsCollectedObject(hitObject.transform.position))
                Gizmos.color = Color.red;
            else
                Gizmos.color = Color.green;
            Gizmos.DrawLine(_cursor.transform.position, hitObject.transform.position);
        }
    }
}
