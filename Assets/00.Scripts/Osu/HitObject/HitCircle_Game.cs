using DG.Tweening;
using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCircle_Game : HitObject_Game
{
    private float _preemptTime = 0f;
    private float _fadeinTime = 0f;
    private Sequence _fadeSeq = null;

    public override void Init(HitObject hitObjectData, Beatmap beatmap, double createTime, float offset)
    {
        base.Init(hitObjectData, beatmap, createTime, offset);
        // animation
        // StartTime + preemptDuration > EndTime 일 때 뭔가 조정 필요
        // 오차에 따라 Time 다르게 변경
        var preemptDuration = beatmap.GetAnimationPreemptDuration();
        var fadeinDuration = beatmap.GetHitObjectFadeInDuration();

        _preemptTime = (float)preemptDuration * 0.001f;
        _fadeinTime = (float)fadeinDuration * 0.001f;

        AnimationStart();
    }

    public override void PushInit()
    {
        base.PushInit();
        KillAnimation();
    }

    public override void AnimationStart()
    {
        base.AnimationStart();
        KillAnimation();
        _approachCircle.KillAnimation();

        FadeAnimation(_fadeinTime);
        _approachCircle.FadeAnimation(_fadeinTime);
        _approachCircle.SizingAnimation(Vector3.one * 5f, Vector3.one * 1f, _preemptTime);
    }

    private void KillAnimation()
    {
        _fadeSeq?.Kill();
    }

    private void FadeAnimation(float duration)
    {
        _fadeSeq?.Kill();
        _fadeSeq = DOTween.Sequence();
        spriteRenderer.color = noAlphaColor;
        _fadeSeq.Append(spriteRenderer.DOFade(1f, duration));
    }

    protected override void JudgementAction(double hitTime, EJudgement judgement)
    {
        base.JudgementAction(hitTime, judgement);

        JudgementPopupUtility.Popup(transform.position + Vector3.up * 0.15f, judgement);
        AudioPool.PopAudio(EAudioType.HitNormal);
        Debug.Log($"hit Error {GetHitError(hitTime)}");

        _isDisable = true;
        Debug.Log(judgement);
    }

    public override void SetHitTime()
    {
        _hitTime = _hitObjectData.StartTime + _offset;
    }
}
