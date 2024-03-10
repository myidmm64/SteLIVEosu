using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatmapPlayer : MonoBehaviour
{
    [SerializeField]
    private CursorObject _cursor = null;
    [SerializeField]
    private OsuPlayField _playField = null;
    [SerializeField]
    private AudioSource _bgm = null;
    private Beatmap _beatmap = null;
    public Beatmap beatmap => _beatmap;

    // Timer
    private int _currentIndex = 0;
    private double _currentTime = 0;
    private double _currentMs = 0;
    public double CurrentMs => _currentMs;

    // Create HitObject Setting
    private List<HitObjectData> _hitObjectDatas = new List<HitObjectData>();
    private double _preemptDuration = 0;

    [SerializeField]
    private Queue<HitObject> _hitObjects = new Queue<HitObject>();

    public void PlayBeatmap()
    {
        _bgm.Play();
        string songName = DevelopHelperObj.Instance.songSelectDropdown.options
            [DevelopHelperObj.Instance.songSelectDropdown.value].text;
        _beatmap = new Beatmap(songName);
        _beatmap.LoadOsuFile();

        _hitObjectDatas = _beatmap.osuFile.hitObject.hitObjectDatas;

        _preemptDuration = BeatmapUtility.GetApproachAnimationDuration(_beatmap);
    }

    private void Update()
    {
        if(_hitObjects.Count > 0)
        {
            _hitObjects.Peek().TestColor(Color.red);
        }

        if (!_bgm.isPlaying) return;
        if (_currentIndex >= _hitObjectDatas.Count) return;

        _currentTime += Time.deltaTime;
        _currentMs = TimeSpan.FromSeconds(_currentTime).TotalMilliseconds;
        if (_hitObjectDatas[_currentIndex].hitTime - _preemptDuration < _currentMs)
        {
            HitObject hitObject = PoolManager.Instance.Pop(EPoolType.HitObject) as HitObject;
            Vector2 hitObjectPosition = _playField.OsuPixelToWorldPosition(new Vector2Int(_hitObjectDatas[_currentIndex].x, _hitObjectDatas[_currentIndex].y));
            Vector2 hitObjectScale = Vector2.one * BeatmapUtility.GetCircleRadius(_beatmap);
            hitObject.Init(this, _hitObjectDatas[_currentIndex].hitTime, hitObjectPosition, hitObjectScale);
            _currentIndex++;
            _hitObjects.Enqueue(hitObject);
        }

        CheckInput();
    }

    private void CheckInput()
    {
        if (!IsInputStarted()) return;
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

    private bool IsInputStarted()
    {
        List<KeyCode> keys = new List<KeyCode>() { KeyCode.Z, KeyCode.X, KeyCode.Mouse0, KeyCode.Mouse1 };
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key)) return true;
        }
        return false;
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
