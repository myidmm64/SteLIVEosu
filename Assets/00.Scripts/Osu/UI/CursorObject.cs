using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorObject : MonoBehaviour
{
    public float collectRadius = 0.38f;

    private Vector3 _position = Vector3.zero;
    private Vector3 _mousePosition = Vector3.zero;

    private void Start()
    {
        transform.position = Vector3.zero;
        _position = transform.position;
    }

    private void Update()
    {
        _mousePosition = Utility.Cam.ScreenToWorldPoint(Input.mousePosition);
        _position.x = _mousePosition.x;
        _position.y = _mousePosition.y;
        transform.position = _position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}
