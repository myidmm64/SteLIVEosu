using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCircle_Game : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;
    private SpriteRenderer spriteRenderer
    {
        get
        {
            if(_spriteRenderer == null )
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return _spriteRenderer;
        }
    }
    private Color noAlphaColor
    {
        get
        {
            Color color = spriteRenderer.color;
            color.a = 0f;
            return color;
        }
    }

    private Sequence _fadeSeq = null;
    private Sequence _sizingSeq = null;

    public void KillAnimation()
    {
        _fadeSeq?.Kill();
        _sizingSeq?.Kill();
    }

    public void FadeAnimation(float duration)
    {
        _fadeSeq?.Kill();
        _fadeSeq = DOTween.Sequence();
        spriteRenderer.color = noAlphaColor;
        _fadeSeq.Append(spriteRenderer.DOFade(1f, duration));
    }

    public void SizingAnimation(Vector3 startSize, Vector3 endSize, float duration)
    {
        _sizingSeq?.Kill();
        _sizingSeq = DOTween.Sequence();
        transform.localScale = startSize;
        _sizingSeq.Append(transform.DOScale(endSize, duration));
    }
}
