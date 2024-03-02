using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OsuPlayField : MonoBehaviour
{
    [SerializeField]
    private RectTransform _osuPixelPositionTrm = null;
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    public Vector3 OsuPixelToWorldPosition(Vector2Int osuPixelPosition)
    {
        _osuPixelPositionTrm.anchoredPosition = new Vector2Int(osuPixelPosition.x, -osuPixelPosition.y);
        return _osuPixelPositionTrm.position;
    }
}
