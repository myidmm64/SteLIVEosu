using OsuParsers.Beatmaps.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CursorObject _cursorObject = null;
    public CursorObject cursorObject => _cursorObject;

    public void OnHit(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            if (context.started)
            {
                // 버튼을 누른 직후
                BeatmapPlayer.Instance.Collect();
            }
            else
            {
                // 버튼을 누르고있음.
            }
        }
    }
}
