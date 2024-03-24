using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class OsuFileSection
{
    protected OsuFileSectionData _sectionData;
    protected string _beatmapDirectory = "";

    public OsuFileSection(OsuFileSectionData sectionData, string beatmapDirectory)
    {
        _sectionData = sectionData;
        _beatmapDirectory = beatmapDirectory;
    }
    public abstract void Read(string line);
    public abstract void SetProperties();
}

public class OsuFileSection_General : OsuFileSection
{
    private const string splitStr = ": ";
    private Dictionary<string, string> _keyValueDic = new Dictionary<string, string>();

    public OsuFileSection_General(OsuFileSectionData sectionData, string beatmapDirectory) : base(sectionData, beatmapDirectory)
    {
    }

    public override void Read(string line)
    {
        int keyValueIndex = line.LastIndexOf(splitStr);
        if (keyValueIndex != -1)
        {
            string keyString = line.Substring(0, keyValueIndex);
            string valueString = line.Substring(keyValueIndex + 2);
            _keyValueDic.Add(keyString, valueString);
        }
    }

    public override void SetProperties()
    {
        OsuFileSectionData_General generalData = _sectionData as OsuFileSectionData_General;
        string outValue = "";
        if (_keyValueDic.TryGetValue("AudioFilename", out outValue)) generalData.audioFileName = outValue;
        if (_keyValueDic.TryGetValue("AudioLeadIn", out outValue)) generalData.audioLeadIn = double.Parse(outValue);
        if (_keyValueDic.TryGetValue("PreviewTime", out outValue)) generalData.previewTime = int.Parse(outValue);
        _keyValueDic.Clear();
    }
}

public class OsuFileSection_Metadata : OsuFileSection
{
    private const string splitStr = ":";
    private Dictionary<string, string> _keyValueDic = new Dictionary<string, string>();

    public OsuFileSection_Metadata(OsuFileSectionData sectionData, string beatmapDirectory) : base(sectionData, beatmapDirectory)
    {
    }

    public override void Read(string line)
    {
        int keyValueIndex = line.LastIndexOf(splitStr);
        if (keyValueIndex != -1)
        {
            string keyString = line.Substring(0, keyValueIndex);
            string valueString = line.Substring(keyValueIndex + 1);
            _keyValueDic.Add(keyString, valueString);
        }
    }

    public override void SetProperties()
    {
        OsuFileSectionData_Metadata metaData = _sectionData as OsuFileSectionData_Metadata;
        string outValue = "";
        if (_keyValueDic.TryGetValue("Title", out outValue)) metaData.title = outValue;
        _keyValueDic.Clear();
    }
}

public class OsuFileSection_Events : OsuFileSection
{
    private bool isBackgroundLine = false;
    private string _backgroundFileName = "";

    public OsuFileSection_Events(OsuFileSectionData sectionData, string beatmapDirectory) : base(sectionData, beatmapDirectory)
    {
    }

    public override void Read(string line)
    {
        if (isBackgroundLine)
        {
            string[] split = line.Split(",");
            string fileName = split[2].Replace("\"", "");
            string filePath = Path.GetFullPath(Path.Combine(_beatmapDirectory, fileName));
            if (File.Exists(filePath))
            {
                _backgroundFileName = fileName;
            }
        }
        isBackgroundLine = line.Equals("//Background and Video events");
    }

    public override void SetProperties()
    {
        OsuFileSectionData_Events eventsData = _sectionData as OsuFileSectionData_Events;
        eventsData.backgroundFileName = _backgroundFileName;
    }
}

public class OsuFileSection_Difficulty : OsuFileSection
{
    private const string splitStr = ":";
    private Dictionary<string, string> _keyValueDic = new Dictionary<string, string>();

    public OsuFileSection_Difficulty(OsuFileSectionData sectionData, string beatmapDirectory) : base(sectionData, beatmapDirectory)
    {
    }

    public override void Read(string line)
    {
        int keyValueIndex = line.LastIndexOf(splitStr);
        if (keyValueIndex != -1)
        {
            string keyString = line.Substring(0, keyValueIndex);
            string valueString = line.Substring(keyValueIndex + 1);
            _keyValueDic.Add(keyString, valueString);
        }
    }

    public override void SetProperties()
    {
        OsuFileSectionData_Difficulty difficultyData = _sectionData as OsuFileSectionData_Difficulty;
        string outValue = "";
        if (_keyValueDic.TryGetValue("HPDrainRate", out outValue)) difficultyData.hpDrainRate = float.Parse(outValue);
        if (_keyValueDic.TryGetValue("CircleSize", out outValue)) difficultyData.circleSize = float.Parse(outValue);
        if (_keyValueDic.TryGetValue("OverallDifficulty", out outValue)) difficultyData.overallDifficulty = float.Parse(outValue);
        if (_keyValueDic.TryGetValue("ApproachRate", out outValue)) difficultyData.approachRate = float.Parse(outValue);
        if (_keyValueDic.TryGetValue("SliderMultiplier", out outValue)) difficultyData.sliderMultiplier = float.Parse(outValue);
        if (_keyValueDic.TryGetValue("SliderTickRate", out outValue)) difficultyData.sliderTickRate = float.Parse(outValue);
        _keyValueDic.Clear();
    }
}

public class OsuFileSection_HitObjects : OsuFileSection
{
    private List<SHitObjectData> _hitObjectDatas = new List<SHitObjectData>();

    public OsuFileSection_HitObjects(OsuFileSectionData sectionData, string beatmapDirectory) : base(sectionData, beatmapDirectory)
    {
    }

    public override void Read(string line)
    {
        SHitObjectData hitObjectData = new SHitObjectData();
        string[] rows = line.Split('|');
        string baseRow = rows[0];
        string[] baseRowColumns = baseRow.Split(',');
        hitObjectData.x = int.Parse(baseRowColumns[0]);
        hitObjectData.y = int.Parse(baseRowColumns[1]);
        hitObjectData.hitTime = int.Parse(baseRowColumns[2]);
        hitObjectData.hitObjectType = (HitObjectType)int.Parse(baseRowColumns[3]);
        hitObjectData.hitSoundType = (HitSoundType)int.Parse(baseRowColumns[4]);
        /*
        string lastColumn = baseRowColumns[baseRowColumns.Length - 1];
        char lastChar = lastColumn[lastColumn.Length - 1];
        if (!char.IsDigit(lastChar)) // Is Slider
        {
            hitObjectData.sliderType = GetSliderTypeWithChar(lastChar);
            List<SSliderTailData> silderTailDatas = new List<SSliderTailData>();
            SSliderTailData sliderTailData = new SSliderTailData();
            for(int i = 1;  i < rows.Length; i++)
            {
                string[] currentColumns = rows[i].Split(',');
                if(currentColumns.Length == 1) // CurvePoint
                {

                    continue;
                }
            }
        }
        */
        _hitObjectDatas.Add(hitObjectData);
    }

    public SliderType GetSliderTypeWithChar(char sliderTypeChar) => sliderTypeChar switch
    {
        'B' => SliderType.Basier,
        'C' => SliderType.Centripetal,
        'L' => SliderType.Line,
        'P' => SliderType.PerpectCircle,
        _ => throw new System.NotImplementedException(),
    };

    public override void SetProperties()
    {
        OsuFileSectionData_HitObjects hitObjectData = _sectionData as OsuFileSectionData_HitObjects;
        hitObjectData.hitObjectDatas = _hitObjectDatas;
    }
}