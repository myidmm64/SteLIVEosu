using System.Collections.Generic;

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

public class OsuFileSectionData_HitObjects : OsuFileSectionData
{
    public struct HitObjectData
    {
        int x;
        int y;
        int hitTime;
    }

    public List<HitObjectData> hitObjectDatas = new List<HitObjectData>();
}