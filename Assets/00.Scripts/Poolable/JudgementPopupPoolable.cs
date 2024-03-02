using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgementPopupPoolable : PoolableObject
{
    private Sequence _animationSeq = null;
    private SpriteRenderer _spriteRenderer = null;

    [SerializeField]
    private float _startSize = 0.2f;
    [SerializeField]
    private float _sizingDuration = 0.1f;
    [SerializeField]
    private float _fadeinDuration = 0.1f;
    [SerializeField]
    private float _fadeoutDuration = 0.6f;

    public override void PopInit()
    {
        PopupAnimation();
    }

    private void PopupAnimation()
    {
        if (_animationSeq != null) _animationSeq.Kill();
        transform.localScale = Vector2.one * _startSize;
        Color noAlphaColor = Color.white;
        noAlphaColor.a = 0f;
        _spriteRenderer.color = noAlphaColor;

        _animationSeq = DOTween.Sequence();
        _animationSeq.Append(transform.DOScale(1f, _sizingDuration));
        _animationSeq.Join(_spriteRenderer.DOFade(1f, _fadeinDuration));
        _animationSeq.Append(_spriteRenderer.DOFade(0f, _fadeoutDuration));
        _animationSeq.AppendCallback(() =>
        {
            PoolManager.Instance.Push(this);
        });
    }

    public override void PushInit()
    {
    }

    public override void StartInit()
    {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }
}
