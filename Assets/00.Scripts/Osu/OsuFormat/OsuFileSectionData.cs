using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class OsuFileSectionData
{

}

[System.Serializable]
public class OsuFileSectionData_General : OsuFileSectionData
{
    public string audioFileName;    // Osu 디렉터리 내 곡 파일 이름
    public double audioLeadIn;      // 해당 ms 기다린 뒤 곡 실행
    public int previewTime;         // 선택창 곡 프리뷰 타임
}

[System.Serializable]
public class OsuFileSectionData_Metadata : OsuFileSectionData
{
    public string title;
}

[System.Serializable]
public class OsuFileSectionData_Events : OsuFileSectionData
{
    public string backgroundFileName;
}

[System.Serializable]
public class OsuFileSectionData_Difficulty : OsuFileSectionData
{
    public float hpDrainRate;
    public float circleSize;
    public float overallDifficulty;
    public float approachRate;
    public float sliderMultiplier;
    public float sliderTickRate;
}

[System.Serializable]
public class OsuFileSectionData_HitObjects : OsuFileSectionData
{

    public List<SHitObjectData> hitObjectDatas = new List<SHitObjectData>();
}

[System.Serializable]
public struct SHitObjectData
{
    public int x;
    public int y;
    public int hitTime;
    public HitObjectType hitObjectType;
    public HitSoundType hitSoundType;
    public SliderType sliderType;
    public SHitSample hitSample;
    public List<SSliderTailData> silderTailDatas;
}

[System.Serializable]
public struct SSliderTailData
{
    public List<Vector2Int> curvePoints;
    public int slides;
    public float length;
    public List<int> edgeSounds;
    public List<string> edgeSets;
    public List<SHitSample> hitSamples;
}

[System.Serializable]
public struct SHitSample
{
    public int normalSet;
    public int additionSet;
    public int index;
    public int volume;
    public string fileName;
}

[System.Serializable]
public enum HitSoundType
{
    Normal,
    Whistle,
    Finish,
    Clap
}

[System.Serializable]
public enum HitObjectType
{
    Circle,
    Slider,
    NewCombo,
    Spinner,
    NewComboWithHexOne,
    NewComboWithHexTwo,
    NewComboWithHexThree,
}

[System.Serializable]
public enum SliderType
{
    Basier,
    Centripetal,
    Line,
    PerpectCircle,
}