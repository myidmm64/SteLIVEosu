using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuPlayField : MonoSingleTon<OsuPlayField>
{
    private RectTransform _osuPixelPositionTrm = null;

    public Vector3 OsuPixelToWorldPosition(Vector2 osuPixelPosition)
    {
        if(_osuPixelPositionTrm == null)
            _osuPixelPositionTrm = transform.Find("PlayField").Find("Position").GetComponent<RectTransform>();

        _osuPixelPositionTrm.anchoredPosition = new Vector2(osuPixelPosition.x, -osuPixelPosition.y);
        return _osuPixelPositionTrm.position;
    }
}
