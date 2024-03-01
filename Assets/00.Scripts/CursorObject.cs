using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    private Camera _cam;
    private Vector3 _position = Vector3.zero;
    private Vector3 _mousePosition = Vector3.zero;

    private bool _cursorVisible = false;

    private void Start()
    {
        _cam = Camera.main;
        _position = transform.position;
    }

    private void Update()
    {
        _mousePosition = _cam.ScreenToWorldPoint(Input.mousePosition);
        _position.x = _mousePosition.x;
        _position.y = _mousePosition.y;
        transform.position = _position;

        if(Input.GetKeyDown(KeyCode.Q))
        {
            _cursorVisible = !_cursorVisible;
            Cursor.visible = _cursorVisible;
        }
    }
}
