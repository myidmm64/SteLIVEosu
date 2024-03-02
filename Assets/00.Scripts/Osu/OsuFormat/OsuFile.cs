using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OsuFile
{
    public string formatVersion = "v14";
    public OsuFileSectionData_General general;
    public OsuFileSectionData_Metadata metadata;
    public OsuFileSectionData_Events events;
    public OsuFileSectionData_Difficulty difficulty;
    public OsuFileSectionData_HitObjects hitObject;

    public OsuFile()
    {
        general = new OsuFileSectionData_General();
        metadata = new OsuFileSectionData_Metadata();
        events = new OsuFileSectionData_Events();
        difficulty = new OsuFileSectionData_Difficulty();
        hitObject = new OsuFileSectionData_HitObjects();
    }
}
