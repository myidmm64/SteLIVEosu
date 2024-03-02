using System.Collections.Generic;

public abstract class OsuFileSectionData
{

}

[System.Serializable]
public class OsuFileSectionData_General : OsuFileSectionData
{
    public string audioFileName;    // Osu ���͸� �� �� ���� �̸�
    public double audioLeadIn;      // �ش� ms ��ٸ� �� �� ����
    public int previewTime;         // ����â �� ������ Ÿ��
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

    public List<HitObjectData> hitObjectDatas = new List<HitObjectData>();
}

[System.Serializable]
public struct HitObjectData
{
    public int x;
    public int y;
    public int hitTime;
}