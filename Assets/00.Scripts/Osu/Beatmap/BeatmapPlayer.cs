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

public class BeatmapPlayer : MonoSingleTon<BeatmapPlayer>
{
    [SerializeField]
    private float _offset = 0;

    private Beatmap _currentBeatmap = null;

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
    //private List<SHitObjectData> _hitObjectDatas = new List<SHitObjectData>();
    private double _preemptDuration = 0;

    [SerializeField]
    private Queue<HitObject_Game> _hitObjects = new Queue<HitObject_Game>();

    public void PlayBeatmap()
    {
        string sn = DevelopHelperObj.Instance.songSelectDropdown.options
            [DevelopHelperObj.Instance.songSelectDropdown.value].text;
        string _beatmapDirectory = Path.GetFullPath(Path.Combine(SongUtility.ExtenalSongDirectory, sn));
        string osuFilePath = Path.GetFullPath(Path.Combine(_beatmapDirectory, $"{sn}.osu"));
        _currentBeatmap = BeatmapDecoder.Decode(osuFilePath);

        foreach (var hitObject in _currentBeatmap.HitObjects)
        {
            if(hitObject is Slider)
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

        /*

        foreach (var hitObject in _hitObjects)
        {
            PoolManager.Instance.Push(hitObject);
        }
        _hitObjects.Clear();
        _currentTime = 0;
        _currentMs = 0;
        _currentIndex = 0;

        _bgm.Play();
        string songName = DevelopHelperObj.Instance.songSelectDropdown.options
            [DevelopHelperObj.Instance.songSelectDropdown.value].text;
        _beatmap = new Beatmap(songName);
        _beatmap.LoadOsuFile();

        _hitObjectDatas = _beatmap.osuFile.hitObject.hitObjectDatas;

        _preemptDuration = BeatmapUtility.GetApproachAnimationDuration(_beatmap);

        */
    }

    private void Update()
    {
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
