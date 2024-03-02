using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using static TMPro.TMP_Dropdown;

public class DevelopHelperObj : MonoSingleTon<DevelopHelperObj>
{
    public GameObject developUICanvas;
    public GameObject playFieldObject;
    public TMP_Dropdown songSelectDropdown;

    private void Start()
    {
        Cursor.visible = false;

        string[] songNames = Directory.GetDirectories(Utility.SongDirectory, "*", SearchOption.TopDirectoryOnly)
            .Select(x => Path.GetFileName(x)).ToArray();
        List<OptionData> songNameOptionDatas = new List<OptionData>();
        foreach (var songName in songNames)
        {
            OptionData songNameOptionData = new OptionData();
            songNameOptionData.text = songName;
            songNameOptionDatas.Add(songNameOptionData);
            songSelectDropdown.AddOptions(songNameOptionDatas);
        }
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
