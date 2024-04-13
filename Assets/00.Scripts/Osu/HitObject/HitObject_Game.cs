using DG.Tweening;
using OsuParsers.Beatmaps.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EJudgement
{
    None,   //miss 범위를 벗어남
    Great,  //300
    Ok,     //100
    Meh,    //50
    Miss    //miss
}

public abstract class HitObject_Game : PoolableObject
{
    private SpriteRenderer _spriteRenderer = null;
    protected SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }
    protected Color noAlphaColor
    {
        get
        {
            Color color = spriteRenderer.color;
            color.a = 0f;
            return color;
        }
    }

    private HitObject _hitObjectData = null;
    private BeatmapPlayer _beatmapPlayer = null;
    private Vector2 _position = Vector2.zero;
    public Vector2 Position => _position;

    protected EJudgement _judgement = EJudgement.None;

    private double _hitTime = 0;

    protected ApproachCircle_Game _approachCircle = null;

    public void TestColor(Color targetColor)
    {
        Color myColor = _spriteRenderer.color;
        targetColor.a = myColor.a;
        _spriteRenderer.color = targetColor;
    }

    public override void PopInit()
    {
    }

    public override void PushInit()
    {
        _spriteRenderer.DOKill();
        TestColor(Color.white);
    }

    public override void StartInit()
    {
        _approachCircle = transform.Find("ApproachCircle").GetComponent<ApproachCircle_Game>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Init(HitObject hitObjectData, BeatmapPlayer beatmapPlayer, Vector2 pos, Vector2 circleSize, double preemptDuration, double fadeinDuration)
    {
        _hitObjectData = hitObjectData;
        _beatmapPlayer = beatmapPlayer;
        _position = pos;

        transform.position = _position;
        transform.localScale = circleSize;
    }

    public virtual void AnimationStart()
    {

    }

    public virtual void ShakeAnimation()
    {
        Debug.Log("Shake!!");
        // transform.DOShakePosition(0.1f);
    }

    public EJudgement JudgementCalculate(double hitTime)
    {
        float od = 0f;//_beatmapPlayer.beatmap.osuFile.difficulty.overallDifficulty;

        double hitError = hitTime - _hitTime;
        if (hitError < 0) hitError *= -1;
        Debug.Log($"Hit Error {hitError}");

        if (hitError < 80 - 6 * od)
            _judgement = EJudgement.Great;
        else if (hitError < 140 - 8 * od)
            _judgement = EJudgement.Ok;
        else if (hitError < 200 - 10 * od)
            _judgement = EJudgement.Meh;
        else if (hitError < 400)
            _judgement = EJudgement.Miss;
        else
        {
            _judgement = EJudgement.None;
            return _judgement;
        }

        // ? 이거 로직 왜 이럼 ㅋㅋ

        transform.DOKill();
        JudgementPopupUtility.Popup(transform.position + Vector3.up * 0.15f, _judgement);
        AudioPool.PopAudio(EAudioType.HitNormal);

        _beatmapPlayer.DequeueObject();
        PoolManager.Instance.Push(this);
        Debug.Log(_judgement);

        // 400보다 오차가 클 때, 덜덜 애니메이션
        return _judgement;
    }
}
