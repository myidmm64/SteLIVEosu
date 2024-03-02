using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevelopHelperObj : MonoBehaviour
{
    public GameObject developUICanvas;
    public GameObject playFieldObject;

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
