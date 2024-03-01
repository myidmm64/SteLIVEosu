using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteContainer : MonoBehaviour
{
    [SerializeField]
    private int bpm = 120;
    double currentTime;

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= 60d / bpm)
        {
            GameObject note = PoolManager.Instance.Pop(EPoolType.Note).gameObject;
            currentTime = 60d / bpm;
            note.transform.SetParent(null);
            note.transform.position = transform.position;
        }
    }
}
