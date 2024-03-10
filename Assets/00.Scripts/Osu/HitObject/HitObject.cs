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
    private BeatmapPlayer _beatmapPlayer = null;
    private Vector2 _pos = Vector2.zero;
    public Vector2 Pos => _pos;
    protected EJudgement _judgement = EJudgement.None;

    private ApproachCircle _approachCircle = null;
    private SpriteRenderer _spriteRenderer = null;
    private double _hitTime = 0;

    private Sequence _circleAnimationSeq = null;
    private Sequence _approachAnimationSeq = null;

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
        _approachCircle = transform.Find("ApproachCircle").GetComponent<ApproachCircle>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void Init(BeatmapPlayer beatmapPlayer, double hitTime, Vector2 pos, Vector2 circleSize)
    {
        _beatmapPlayer = beatmapPlayer;
        _hitTime = hitTime;
        _pos = pos;

        transform.position = _pos;
        transform.localScale = circleSize;

        Color noAlphaColor = Color.white;
        noAlphaColor.a = 0f;
        _spriteRenderer.color = noAlphaColor;
        _approachCircle.transform.localScale = Vector3.one * 5f;
        _approachCircle.spriteRenderer.color = noAlphaColor;

        // 추후 double로 수정
        float preemptTime = (float)BeatmapUtility.GetApproachAnimationDuration(beatmapPlayer.beatmap) * 0.001f;
        float fadeinTime = (float)BeatmapUtility.GetHitObjectFadeInDuration(beatmapPlayer.beatmap) * 0.001f;
        CircleAnimation(fadeinTime);
        ApproachAnimation(fadeinTime, preemptTime);
    }

    public virtual void ShakeAnimation()
    {
        Debug.Log("Shake!!");
        transform.DOShakePosition(0.1f);
    }

    public virtual void PreemptAnimation()
    {

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
            /*
            _beatmapPlayer.DequeueObject();
            PoolManager.Instance.Push(this);
            */
        });
    }

    private void ApproachAnimation(float fadeTime, float preemptTime)
    {
        if (_approachAnimationSeq != null) _approachAnimationSeq.Kill();
        _approachAnimationSeq = DOTween.Sequence();

        _approachAnimationSeq.Append(_approachCircle.spriteRenderer.DOFade(1f, fadeTime)).SetEase(Ease.Linear);
        _approachAnimationSeq.Join(_approachCircle.transform.DOScale(1f, preemptTime)).SetEase(Ease.Linear);
        _approachAnimationSeq.Append(_approachCircle.spriteRenderer.DOFade(0f, fadeTime)).SetEase(Ease.Linear)
            .Join(_approachCircle.transform.DOScale(1.5f, fadeTime)).AppendCallback(() =>
            {
                transform.DOKill();
                _circleAnimationSeq.Kill();
                _approachAnimationSeq.Kill();
                _circleAnimationSeq = null;
                _approachAnimationSeq = null;
                _beatmapPlayer.DequeueObject();
                PoolManager.Instance.Push(this);
            }); ;
    }

    public EJudgement JudgementCalculate(double hitTime)
    {
        float od = _beatmapPlayer.beatmap.osuFile.difficulty.overallDifficulty;

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
        _circleAnimationSeq.Kill();
        _approachAnimationSeq.Kill();
        _circleAnimationSeq = null;
        _approachAnimationSeq = null;
        JudgementPopupUtility.Popup(transform.position + Vector3.up * 0.15f, _judgement);
        AudioPool.PopAudio(EAudioType.HitNormal);

        _beatmapPlayer.DequeueObject();
        PoolManager.Instance.Push(this);
        Debug.Log(_judgement);

        // 400보다 오차가 클 때, 덜덜 애니메이션
        return _judgement;
    }
}
