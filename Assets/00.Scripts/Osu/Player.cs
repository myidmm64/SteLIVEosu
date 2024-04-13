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
                // ��ư�� ���� ����
                BeatmapPlayer.Instance.Collect();
            }
            else
            {
                // ��ư�� ����������.
            }
        }
    }
}
