using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EJudgement
{
    None,   //miss 범위를 벗어남
    Great,  //300
    Ok,     //100
    Meh,    //50
    Miss    //miss
}

public abstract class HitObject : PoolableObject
{
    private ApproachCircle _approachCircle = null;
    private double _hitTime = 0;

    public override void PopInit()
    {
    }

    public override void PushInit()
    {
    }

    public override void StartInit()
    {
        _approachCircle = transform.Find("ApproachCircle").GetComponent<ApproachCircle>(); 
    }

    public void InitHitObject(double hitTime, float approachTime)
    {
        _hitTime = hitTime;
        //_approachCircle.AnimationStart(approachTime);
        StartCoroutine(WaitAndPush(approachTime + 0.05f));
    }

    public EJudgement JudgementCalculate(double hitTime, int od)
    {
        double hitError = hitTime - _hitTime;
        if (hitError < 0) hitError *= -1;
        Debug.Log($"Hit Error {hitError}");

        if (hitError < 80 - 6 * od)
            return EJudgement.Great;
        else if (hitError < 140 - 8 * od)
            return EJudgement.Ok;
        else if (hitError < 200 - 10 * od)
            return EJudgement.Meh;
        else if (hitError < 400)
            return EJudgement.Miss;

        return EJudgement.None;
    }

    private IEnumerator WaitAndPush(float time)
    {
        yield return new WaitForSeconds(time);
        AudioPool.PopAudio(EAudioType.HitNormal);
        PoolManager.Instance.Push(this);
    }
}
