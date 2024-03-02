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
    public int SliderMultiplier; // slider�� �ӵ� (100�ȼ��� ���� ��Ʈ)
    public int SliderTickRate; // ��Ʈ�� �����̴� ƽ�� ��
}
