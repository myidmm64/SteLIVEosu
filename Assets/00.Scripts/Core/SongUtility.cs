using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    private static List<string> _defaultSongs = null; 
    public static bool IsDefaultSong(string songName)
    {
        if(_defaultSongs == null)
        {
            Debug.Log("Set Default Song Directory");
            _defaultSongs = new List<string>();
            var defaultSongDirectories = Directory.GetDirectories(_defaultSongDirectory, "*", SearchOption.TopDirectoryOnly);
            foreach(var defaultSongDirectory in defaultSongDirectories)
            {
                string songDirectoryName = Path.GetFileName(defaultSongDirectory);
                _defaultSongs.Add(songDirectoryName);
            }
        }
        return _defaultSongs.Contains(songName);
    }

    public static AudioClip LoadBGM(SelectOsuFileData osuFileData) // 로컬 파일 노래 read
    {
        string directoryPath = osuFileData.OsuDirectoryPath;
        string directoryName = osuFileData.OsuDirectoryName;
        string musicName = osuFileData.AudioFileName;

        bool isDefaultSong = IsDefaultSong(directoryName);
        if (isDefaultSong)
        {
            musicName = Path.GetFileNameWithoutExtension(musicName);

            string resourcePath = Path.Combine("Songs", directoryName, musicName);
            AudioClip clip = Resources.Load<AudioClip>(resourcePath);
            return clip;
        }
        else
        {

        }
        return null;
    }
}
