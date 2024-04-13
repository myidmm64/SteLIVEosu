using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

using OsuParsers.Beatmaps;
using OsuParsers.Decoders;

/// <summary>
/// Select Scene에서 Osu 파일을 설명하기 위한 클래스
/// </summary>
public class SelectOsuFileData
{
    private readonly string _osuFilePath;
    private Beatmap _beatmap = null;

    public string Title => _beatmap.MetadataSection.Title;
    public string Artist => _beatmap.MetadataSection.Artist;
    public string Creator => _beatmap.MetadataSection.Creator;
    public string Version => _beatmap.MetadataSection.Version;

    public string DirectoryName => Path.GetDirectoryName(_osuFilePath);

    public SelectOsuFileData(string osuFilePath)
    {
        _osuFilePath = osuFilePath;
        OsuFileLoad();
    }

    private void OsuFileLoad()
    {
        _beatmap = BeatmapDecoder.Decode(_osuFilePath);
    }
}

