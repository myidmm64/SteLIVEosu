using OsuParsers.Beatmaps;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlider : MonoBehaviour
{
    [ContextMenu("테스트 시작")]
    public void TestStart()
    {
        string osuFileKey = DevelopHelperObj.Instance.songSelectDropdown.options
            [DevelopHelperObj.Instance.songSelectDropdown.value].text;
        SelectOsuFileData selectedFileData = DevelopHelperObj.Instance._developTestSelectOsuFileMap[osuFileKey];

        var testSlider = selectedFileData.beatmap.HitObjects[9];
        HitObject_Game hitObject = PoolManager.Instance.Pop(EPoolType.Slider) as HitObject_Game;
        hitObject.Init(testSlider, selectedFileData.beatmap, 0f, 0f);
    }
}
