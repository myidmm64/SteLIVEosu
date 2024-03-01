using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteContainer : MonoBehaviour
{
    [SerializeField]
    private int bpm = 120;
    [SerializeField]
    private Vector2 _minPosition = new Vector2(-8f, -4.3f);
    [SerializeField]
    private Vector2 _maxPosition = new Vector2(8f, 4.3f);

    double currentTime;

    private void Start()
    {
        
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= 60d / bpm)
        {
            GameObject note = PoolManager.Instance.Pop(EPoolType.Note).gameObject;
            currentTime -= 60d / bpm;
            note.transform.SetParent(null);
            note.transform.position = new Vector2(Random.Range(_minPosition.x, _maxPosition.x), Random.Range(_minPosition.y, _maxPosition.y));
        }
    }
}
