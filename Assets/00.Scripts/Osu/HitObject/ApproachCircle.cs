using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCircle : MonoBehaviour
{
    public void AnimationStart(float duration)
    {
        transform.DOKill();
        transform.transform.localScale = Vector3.one * 1f;
        transform.DOScale(0.3f, duration).SetEase(Ease.Linear);
    }
}
