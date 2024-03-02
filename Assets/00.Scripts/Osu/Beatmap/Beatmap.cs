using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Beatmap
{
    public OsuFile osuFile;
    private readonly string _beatmapDirectory;
    private OsuFormatParser parser;

    public Beatmap(string songName)
    {
        _beatmapDirectory = Path.GetFullPath(Path.Combine(Application.dataPath, "01.Songs", songName));
        parser = new OsuFormatParser();
    }

    public void LoadOsuFile()
    {
        string directoryname = Path.GetFileName(_beatmapDirectory);
        string osuFilePath = Path.GetFullPath(Path.Combine(_beatmapDirectory, $"{directoryname}.osu"));
        osuFile = parser.Parse(osuFilePath);
    }
}
