using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCircle : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;

    public void AnimationStart(float sizingDuration, float fadeDuration)
    {
        transform.DOKill();
        transform.transform.localScale = Vector3.one * 1f;
        transform.DOScale(0.3f, sizingDuration).SetEase(Ease.Linear);
    }
}
