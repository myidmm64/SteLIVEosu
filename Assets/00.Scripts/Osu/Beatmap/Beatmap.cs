using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Beatmap
{
    public OsuFile osuFile;
    private readonly string _beatmapDirectory;
    private readonly string _songName;
    private OsuFormatParser parser;

    public Beatmap(string songName)
    {
        _songName = songName;
        _beatmapDirectory = Path.GetFullPath(Path.Combine(Utility.SongDirectory, songName));
        parser = new OsuFormatParser();
    }

    public void LoadOsuFile()
    {
        string directoryname = Path.GetFileName(_beatmapDirectory);
        string osuFilePath = Path.GetFullPath(Path.Combine(_beatmapDirectory, $"{_songName}.osu"));
        osuFile = parser.Parse(osuFilePath);
    }
}
