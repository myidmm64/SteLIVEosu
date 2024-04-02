using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachCircle : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer = null;
    public SpriteRenderer spriteRenderer
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
}
