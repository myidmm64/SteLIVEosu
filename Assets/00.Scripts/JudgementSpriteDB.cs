using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/JudgementSpriteDB")]
public class JudgementSpriteDB : ScriptableObject
{
    public List<SJudgementSpriteData> judgementSpriteDatas = new List<SJudgementSpriteData>();
}

[System.Serializable]
public struct SJudgementSpriteData
{
    public EJudgement judgement;
    public Sprite sprite;
}