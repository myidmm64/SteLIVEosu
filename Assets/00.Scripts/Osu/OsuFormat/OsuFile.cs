using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuFile
{
    public string formatVersion = "v14";
    public OsuFileSectionData_General general;
    public OsuFileSectionData_Metadata metadata;
    public OsuFileSectionData_Events events;
    public OsuFileSectionData_Difficulty difficulty;
    public OsuFileSectionData_HitObjects hitObject;
}
