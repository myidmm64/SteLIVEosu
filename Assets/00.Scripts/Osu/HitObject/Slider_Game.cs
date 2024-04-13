using OsuParsers.Beatmaps;
using OsuParsers.Beatmaps.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider_Game : HitObject_Game
{
    [SerializeField]
    private GameObject _sldierTail = null;

    public override void Init(HitObject hitObjectData, Beatmap beatmap, double createTime, float offset)
    {
        base.Init(hitObjectData, beatmap, createTime, offset);

        InitSliderLine();
    }

    public override void SetHitTime()
    {
        _hitTime = _hitObjectData.StartTime + _offset;
    }

    public void InitSliderLine()
    {
        Slider sliderData = _hitObjectData as Slider;
        Debug.Log(sliderData.CurveType);
        Vector2 sliderTailPosition = OsuPlayField.Instance.OsuPixelToWorldPosition(new Vector2(sliderData.SliderPoints[sliderData.SliderPoints.Count-1].X, sliderData.SliderPoints[sliderData.SliderPoints.Count - 1].Y));
        _sldierTail.transform.position = sliderTailPosition;

        foreach (var point in sliderData.SliderPoints)
        {
            Debug.Log(point.X + "  " + point.Y);
        }
    }
}

public abstract class SliderLineGenerator
{

}

public class PerpectCircleLineGenerator : SliderLineGenerator
{

}

public class LinearLineGenerator : SliderLineGenerator
{

}

public class BasierLineGenerator : SliderLineGenerator
{

}