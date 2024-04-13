using DG.Tweening;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EJudgement
{
    None,   //miss 범위를 벗어남
    Great,  //300
    Ok,     //100
    Meh,    //50
    Miss,   //miss
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

    protected float _offset = 0f;
    protected HitObject _hitObjectData = null;
    private Vector2 _position = Vector2.zero;
    public Vector2 Position => _position;
    protected Beatmap _beatmap = null;

    protected double _hitTime = 0;
    protected bool _isDisable = false;

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
        //_spriteRenderer.DOKill();
        //TestColor(Color.white);
        _isDisable = false;
    }

    public override void StartInit()
    {
        _approachCircle = transform.Find("ApproachCircle").GetComponent<ApproachCircle_Game>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Init(HitObject hitObjectData, Beatmap beatmap, double createTime, float offset)
    {
        _offset = offset;
        _hitObjectData = hitObjectData;
        _beatmap = beatmap;

        Vector2 hitObjectPosition = OsuPlayField.Instance.OsuPixelToWorldPosition(new Vector2(hitObjectData.Position.X, hitObjectData.Position.Y));
        Vector2 circleSize = Vector2.one * beatmap.GetCircleRadius();
        _position = hitObjectPosition;
        transform.position = _position;
        transform.localScale = circleSize;
        SetHitTime();
    }

    public abstract void SetHitTime();

    public virtual void AnimationStart()
    {

    }

    protected virtual void JudgementAction(double hitTime, EJudgement judgement)
    {

    }

    public virtual void ShakeAnimation()
    {
        Debug.Log("Shake!!");
        // transform.DOShakePosition(0.1f);
    }

    public double GetHitError(double hitTime)
    {
        double hitError = hitTime - _hitTime;
        if (hitError < 0) hitError *= -1;
        return hitError;
    }

    public EJudgement GetJudgement(double hitTime)
    {
        double hitError = GetHitError(hitTime);
        float od = _beatmap.DifficultySection.OverallDifficulty;

        EJudgement judgement = EJudgement.None;
        if (hitError < 80 - 6 * od)
            judgement = EJudgement.Great;
        else if (hitError < 140 - 8 * od)
            judgement = EJudgement.Ok;
        else if (hitError < 200 - 10 * od)
            judgement = EJudgement.Meh;
        else if (hitError < 400)
            judgement = EJudgement.Miss;
        else
            judgement = EJudgement.None;

        return judgement;
    }

    public bool IsDisable()
    {
        if (_isDisable) return true;
        double currentMs = BeatmapPlayer.Instance.CurrentMs;
        EJudgement tempJudgement = GetJudgement(currentMs);
        if(currentMs > _hitTime && (tempJudgement == EJudgement.Miss || tempJudgement == EJudgement.None))
        {
            _isDisable = true;
        }
        return _isDisable;
    }

    public EJudgement CollectObject(double hitTime)
    {
        EJudgement judgement = GetJudgement(hitTime);
        JudgementAction(hitTime, judgement);
        return judgement;
    }
}
