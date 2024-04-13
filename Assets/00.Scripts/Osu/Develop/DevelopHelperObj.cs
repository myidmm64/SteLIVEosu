using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class DevelopHelperObj : MonoSingleTon<DevelopHelperObj>
{
    public GameObject developUICanvas;
    public GameObject playFieldObject;
    public TMP_Dropdown songSelectDropdown;

    public List<SelectOsuFileData> selectOsuFiles = new List<SelectOsuFileData>();
    public Dictionary<string, SelectOsuFileData> _developTestSelectOsuFileMap = new Dictionary<string, SelectOsuFileData>();

    private void Start()
    {
        Cursor.visible = false;

        List<string> songDirectories = new List<string>();
        songDirectories.AddRange(Directory.GetDirectories(SongUtility.DefaultSongDirectory, "*", SearchOption.TopDirectoryOnly));
        songDirectories.AddRange(Directory.GetDirectories(SongUtility.ExtenalSongDirectory, "*", SearchOption.TopDirectoryOnly));

        Parallel.ForEach(songDirectories, songDirectory =>
        {
            var osuFiles = Directory.GetFiles(songDirectory, "*.osu", SearchOption.TopDirectoryOnly);
            if (osuFiles.Length == 0)
            {
                Debug.Log($"{songDirectory} 경로에 osu 파일이 존재하지 않습니다.");
                return;
            }

            foreach (var osuFile in osuFiles)
            {
                SelectOsuFileData selectOsuFileData = new SelectOsuFileData(osuFile);

                lock (selectOsuFiles)
                {
                    selectOsuFiles.Add(selectOsuFileData);
                }

                string textKey = $"{selectOsuFileData.Title}_{selectOsuFileData.Version}";

                lock (_developTestSelectOsuFileMap)
                {
                    _developTestSelectOsuFileMap.Add(textKey, selectOsuFileData);
                }
            }
        });

        songSelectDropdown.ClearOptions();
        List<OptionData> songNameOptionDatas = new List<OptionData>();
        foreach (var developTestSelectOsuFile in _developTestSelectOsuFileMap)
        {
            OptionData songNameOptionData = new OptionData();
            songNameOptionData.text = developTestSelectOsuFile.Key;
            songNameOptionDatas.Add(songNameOptionData);
        }   
        songSelectDropdown.AddOptions(songNameOptionDatas);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            developUICanvas.SetActive(!developUICanvas.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Cursor.visible = !Cursor.visible;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            playFieldObject.SetActive(!playFieldObject.activeSelf);
        }
    }
}
