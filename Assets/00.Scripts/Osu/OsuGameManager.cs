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
    private DefaultAsset _osuFile;
    [SerializeField]
    private int _offset = 0;

    [SerializeField]
    private SHitObjectProperty _testSetting;
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

    private void Start()
    {
        OsuFileParse();
    }

    private void OsuFileParse()
    {
        string realFilePath = AssetDatabase.GetAssetPath(_osuFile);
        Debug.Log(realFilePath);
        if (!File.Exists(realFilePath)) return;
        _startBeats.Clear();
        StreamReader reader = new StreamReader(realFilePath);
        bool hitObjectsStart = false;
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            if (!hitObjectsStart)
            {
                hitObjectsStart = line.Equals("[HitObjects]");
                continue;
            }
            string[] properties = line.Split(",");
            _startBeats.Add(Double.Parse(properties[2]) + _offset);
            _positionPixels.Add(new Vector2Int(int.Parse(properties[0]), int.Parse(properties[1])));
        }
        reader.Close();
    }

    private void Update()
    {
        if (_currentIndex >= _startBeats.Count) return;

        _currentTime += Time.deltaTime;
        _currentMs = TimeSpan.FromSeconds(_currentTime).TotalMilliseconds;
        if (_startBeats[_currentIndex] - 1000 < _currentMs)
        {
            _currentIndex++;
            _hitObject = PoolManager.Instance.Pop(EPoolType.Note) as HitObject;
            _hitObject.InitHitObject(_startBeats[_currentIndex], 1f);
            Vector2 spawnPosition = new Vector2(Random.Range(_minPosition.x, _maxPosition.x), Random.Range(_minPosition.y, _maxPosition.y));
            _hitObject.transform.position = spawnPosition;
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
