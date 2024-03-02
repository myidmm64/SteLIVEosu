using DG.Tweening;
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

public abstract class HitObject : PoolableObject
{
    private ApproachCircle _approachCircle = null;
    private SpriteRenderer _spriteRenderer = null;
    private double _hitTime = 0;

    private Sequence _circleAnimationSeq = null;
    private Sequence _approachAnimationSeq = null;

    public override void PopInit()
    {
    }

    public override void PushInit()
    {
    }

    public override void StartInit()
    {
        _approachCircle = transform.Find("ApproachCircle").GetComponent<ApproachCircle>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitHitObject(double hitTime, double preemptMs, double fadeinMs)
    {
        _hitTime = hitTime;
        float preemptTime = (float)preemptMs * 0.001f;
        float fadeTime = (float)fadeinMs * 0.001f;
        Color noAlphaColor = Color.white;
        noAlphaColor.a = 0f;
        _spriteRenderer.color = noAlphaColor;
        _approachCircle.transform.localScale = Vector3.one * 5f;
        _approachCircle.spriteRenderer.color = noAlphaColor;

        CircleAnimation(fadeTime);
        ApproachAnimation(fadeTime, preemptTime);
    }

    private void CircleAnimation(float fadeTime)
    {
        if (_circleAnimationSeq != null) _circleAnimationSeq.Kill();
        _circleAnimationSeq = DOTween.Sequence();

        _circleAnimationSeq.Append(_spriteRenderer.DOFade(1f, fadeTime)).SetEase(Ease.Linear);
        _circleAnimationSeq.AppendCallback(() =>
        {
            //TODO : Sprite Change
        });
        _circleAnimationSeq.Append(_spriteRenderer.DOFade(0f, fadeTime)).SetEase(Ease.Linear);
        _circleAnimationSeq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    private void ApproachAnimation(float fadeTime, float preemptTime)
    {
        if (_approachAnimationSeq != null) _approachAnimationSeq.Kill();
        _approachAnimationSeq = DOTween.Sequence();

        _approachAnimationSeq.Append(_approachCircle.spriteRenderer.DOFade(1f, fadeTime)).SetEase(Ease.Linear);
        _approachAnimationSeq.Join(_approachCircle.transform.DOScale(1f, preemptTime)).SetEase(Ease.Linear);
        _approachAnimationSeq.AppendCallback(() =>
        {
            AudioPool.PopAudio(EAudioType.HitNormal);
        });
        _approachAnimationSeq.Append(_approachCircle.spriteRenderer.DOFade(0f, fadeTime)).SetEase(Ease.Linear)
            .Join(_approachCircle.transform.DOScale(1.5f, fadeTime));
    }

    public EJudgement JudgementCalculate(double hitTime, int od)
    {
        double hitError = hitTime - _hitTime;
        if (hitError < 0) hitError *= -1;
        Debug.Log($"Hit Error {hitError}");

        if (hitError < 80 - 6 * od)
            return EJudgement.Great;
        else if (hitError < 140 - 8 * od)
            return EJudgement.Ok;
        else if (hitError < 200 - 10 * od)
            return EJudgement.Meh;
        else if (hitError < 400)
            return EJudgement.Miss;

        return EJudgement.None;
    }
}
