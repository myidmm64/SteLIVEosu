using UnityEngine;

// .osu file format version v14
[System.Serializable]
public struct SHitObjectProperty
{
    // Difficulty
    public int HPDrainRate; // 0-10
    public int CircleSize; // 0-10
    public int OverallDifficulty; // 0-10
    public int ApproachRate; // 0-10
    public int SliderMultiplier; // slider의 속도 (100픽셀을 가는 비트)
    public int SliderTickRate; // 비트당 슬라이더 틱의 양
}
