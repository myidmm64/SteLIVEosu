using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class OsuGameManager : MonoSingleTon<OsuGameManager>
{
    [SerializeField]
    private OsuPlayField _playField = null;
    [SerializeField]
    private DefaultAsset _osuFile;
    [SerializeField]
    private int _offset = 0;

    [SerializeField]
    private List<double> _startBeats = new List<double>();
    [SerializeField]
    private List<Vector2Int> _positionPixels = new List<Vector2Int>();

    private int _currentIndex = 0;
    private double _currentTime = 0;
    private double _currentMs = 0;

    HitObject _hitObject;

    private Vector2 _minPosition = new Vector2(-8f, -4.3f);
    private Vector2 _maxPosition = new Vector2(8f, 4.3f);

    private void Update()
    {
        if (_currentIndex >= _startBeats.Count) return;

        _currentTime += Time.deltaTime;
        _currentMs = TimeSpan.FromSeconds(_currentTime).TotalMilliseconds;
        if (_startBeats[_currentIndex] - 1000 < _currentMs)
        {
            _hitObject = PoolManager.Instance.Pop(EPoolType.HitObject) as HitObject;
            _hitObject.InitHitObject(_startBeats[_currentIndex], 1f);
            Vector2 spawnPosition = new Vector2(Random.Range(_minPosition.x, _maxPosition.x), Random.Range(_minPosition.y, _maxPosition.y));
            //_hitObject.transform.position = spawnPosition;
            _hitObject.transform.position = _playField.OsuPixelToWorldPosition(_positionPixels[_currentIndex]);

            _currentIndex++;
        }

        if (_hitObject != null && IsInputStarted())
        {
            _hitObject.JudgementCalculate(_currentMs, 0);
        }
    }

    private bool IsInputStarted()
    {
        List<KeyCode> keys = new List<KeyCode>() { KeyCode.Z, KeyCode.X, KeyCode.Mouse0, KeyCode.Mouse1 };
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key)) return true;
        }
        return false;
    }
}
