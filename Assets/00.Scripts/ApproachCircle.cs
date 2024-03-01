using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCircle : MonoBehaviour
{
    public void AnimationStart(float duration)
    {
        transform.DOKill();
        transform.transform.localScale = Vector3.one * 2f;
        transform.DOScale(1f, duration);
    }
}
