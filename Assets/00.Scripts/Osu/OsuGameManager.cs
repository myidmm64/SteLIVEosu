using OsuParsers.Beatmaps.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuGameManager : MonoSingleTon<OsuGameManager>
{
    [SerializeField]
    private BeatmapPlayer _beatmapPlayer = null;
    private ScoreManager _scoreManager = null;

    public void StartGame()
    {
        string osuFileKey = DevelopHelperObj.Instance.songSelectDropdown.options
            [DevelopHelperObj.Instance.songSelectDropdown.value].text;
        SelectOsuFileData selectedFileData = DevelopHelperObj.Instance._developTestSelectOsuFileMap[osuFileKey];
        _beatmapPlayer.PlayBeatmap(selectedFileData);
        _scoreManager = new ScoreManager(selectedFileData.beatmap);
    }

    public void AddScore(int hitValue) // judgement
    {
        _scoreManager.AddScore(hitValue);
        Debug.Log($"current Score : {_scoreManager.Score}");
    }
}
