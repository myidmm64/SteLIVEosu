using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : PoolableObject
{
    private ApproachCircle _approachCircle = null;

    public override void PopInit()
    {
        _approachCircle.AnimationStart(1f);
        StartCoroutine(Test());
    }

    public override void PushInit()
    {
    }

    public override void StartInit()
    {
        _approachCircle = transform.Find("ApproachCircle").GetComponent<ApproachCircle>(); 
    }

    private IEnumerator Test()
    {
        yield return new WaitForSeconds(1.2f);
        PoolManager.Instance.Push(this);
    }
}
