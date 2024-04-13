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
    public readonly string osuFilePath;
    public readonly Beatmap beatmap = null;

    public string Title => beatmap.MetadataSection.Title;
    public string Artist => beatmap.MetadataSection.Artist;
    public string Creator => beatmap.MetadataSection.Creator;
    public string Version => beatmap.MetadataSection.Version;

    public string AudioFileName => beatmap.GeneralSection.AudioFilename;

    public string OsuDirectoryPath => Path.GetDirectoryName(osuFilePath);
    public string OsuDirectoryName => Path.GetFileName(OsuDirectoryPath);

    public SelectOsuFileData(string osuFilePath)
    {
        this.osuFilePath = osuFilePath;
        beatmap = BeatmapDecoder.Decode(osuFilePath);
    }
}

