using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Beatmap : MonoSingleTon<Beatmap>
{
    [SerializeField]
    private OsuFormatParser parser;
    [SerializeField]
    private OsuFile osuFile;

    [ContextMenu("�Ľ� �׽�Ʈ")]
    public void ParseTest()
    {
        parser = new OsuFormatParser();
        string path = Path.GetFullPath(Path.Combine(Application.dataPath, "01.Songs", "TestOsuFile", "beatmapTest.osu"));
        osuFile = parser.Parse(path);
    }
}
