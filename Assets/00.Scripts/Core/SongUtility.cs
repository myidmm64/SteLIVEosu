using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class SongUtility
{
    private static string _externalSongDirectory = null;
    public static string ExtenalSongDirectory
    {
        get
        {
            if (_externalSongDirectory == null)
            {
                _externalSongDirectory = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, "Songs"));
            }
            return _externalSongDirectory;
        }
    }

    private static string _defaultSongDirectory = null;
    public static string DefaultSongDirectory
    {
        get
        {
            if (_defaultSongDirectory == null)
            {
                _defaultSongDirectory = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, "DefaultSongs"));
            }
            return _defaultSongDirectory;
        }
    }

    private static readonly List<string> _defaultSongs = new List<string>() { "SongSongSongEx" }; 
    public static bool IsDefaultSong(string songName)
    {
        return _defaultSongs.Contains(songName);
    }

    public static void LoadSong(string musicPath) // 로컬 파일 노래 read
    {
    }
}
