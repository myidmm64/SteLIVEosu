using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OsuFormatParser
{
    public OsuFile Parse(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"{filePath} 경로에 파일이 존재하지 않습니다.");
            return null;
        }

        OsuFile osuFile = new OsuFile();
        StreamReader reader = new StreamReader(filePath);
        List<OsuFileSection> sections = new List<OsuFileSection>();
        string beatmapDirectory = Path.GetDirectoryName(filePath);

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (line.Equals("")) continue;
            if (line.StartsWith("[") && line.EndsWith("]"))
            {
                string currentSectionStr = line.Substring(1, line.Length);
                sections.Add(GetOsuFileSection(currentSectionStr, osuFile, beatmapDirectory));
                continue;
            }
            if (sections.Count == 0) continue;
            sections[sections.Count - 1].Read(line);
        }
        reader.Close();
        foreach (OsuFileSection section in sections)
        {
            section.SetProperties();
        }

        return osuFile;
    }

    private OsuFileSection GetOsuFileSection(string sectionStr, OsuFile osuFile, string beatmapDirectory) => sectionStr switch
    {
        "General" => new OsuFileSection_General(osuFile.general, beatmapDirectory),
        "Metadata" => new OsuFileSection_Metadata(osuFile.metadata, beatmapDirectory),
        "Events" => new OsuFileSection_Events(osuFile.events, beatmapDirectory),
        "Difficulty" => new OsuFileSection_Difficulty(osuFile.difficulty, beatmapDirectory),
        "HitObjects" => new OsuFileSection_HitObjects(osuFile.hitObject, beatmapDirectory),
        _ => throw new NotImplementedException(),
    };
}
