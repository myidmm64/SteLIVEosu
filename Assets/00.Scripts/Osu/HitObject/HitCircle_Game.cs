using DG.Tweening;
using OsuParsers.Beatmaps.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCircle_Game : HitObject_Game
{
    private float _preemptTime = 0f;
    private float _fadeinTime = 0f;
    private Sequence _fadeSeq = null;

    public override void Init(HitObject hitObjectData, BeatmapPlayer beatmapPlayer, Vector2 pos, Vector2 circleSize, double preemptDuration, double fadeinDuration)
    {
        base.Init(hitObjectData, beatmapPlayer, pos, circleSize, preemptDuration, fadeinDuration);
        // animation
        // StartTime + preemptDuration > EndTime 일 때 뭔가 조정 필요
        _preemptTime = (float)preemptDuration * 0.001f;
        _fadeinTime = (float)fadeinDuration * 0.001f;

        AnimationStart();
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
}
